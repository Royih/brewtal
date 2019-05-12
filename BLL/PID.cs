using System;
using System.Linq;
using System.Threading;
using Brewtal.Database;
using Brewtal.Dtos;
using Microsoft.Extensions.Logging;

namespace Brewtal.BLL
{
    public class PID
    {
        private readonly int _pidId;
        private readonly string _pidName;
        public readonly PidConfig PidConfig;
        private const double DefaultPIDKp = 200;

        private const double DefaultPIDKi = 200;

        private const double DefaultPIDKd = 10;

        public PidStatusDto Status { get; private set; }

        private double target = 0;

        private readonly PIDRegulator3 _pidRegulator;
        private readonly BrewIO _brewIO;
        private readonly Outputs _output;
        private readonly HeaterController _heater;

        public PID(int pidId, string pidName, BrewIO brewIO, Outputs output)
        {
            _pidId = pidId;
            _pidName = pidName;
            _brewIO = brewIO;
            _output = output;
            _heater = new HeaterController(_brewIO, output);
            _heater.Start();

            using (var db = new BrewtalContext())
            {
                PidConfig = db.PidConfigs.SingleOrDefault(x => x.PidId == pidId);
                if (PidConfig == null)
                {
                    PidConfig = new PidConfig
                    {
                        PidId = pidId,
                        PIDKp = DefaultPIDKp,
                        PIDKi = DefaultPIDKi,
                        PIDKd = DefaultPIDKd
                    };
                    db.Add(PidConfig);
                    db.SaveChanges();
                }
            }
            _pidRegulator = new PIDRegulator3(PidConfig.PIDKp, PidConfig.PIDKi, PidConfig.PIDKd);
            Status = new PidStatusDto
            {
                PidId = _pidId,
                PidName = _pidName
            };

        }

        public void Calculate(TempReaderResultDto currentTempResult)
        {
            var currentTemp = _pidId == 0 ? currentTempResult.Temp1 : currentTempResult.Temp2;
            var outputValue = _pidRegulator.Calculate(currentTemp, Status.TargetTemp);
            _heater.UpdateNextCyclePercentage(outputValue);

            Status.CurrentTemp = currentTemp;
            Status.OutputValue = outputValue;
            Status.Output = _heater.CurrentStatus;
            Status.ErrorSum = _pidRegulator.ErrorSum;
        }

        public void UpdateTargetTemp(double newTargetTemp)
        {
            this.target = newTargetTemp;
            Status.TargetTemp = newTargetTemp;
            _pidRegulator.Reset();
        }

    }
}