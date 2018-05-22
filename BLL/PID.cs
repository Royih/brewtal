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
        public const double PIDKp = 5f; // 

        public const double PIDKi = 0.01f; // 

        public const double PIDKd = 75.0f; //

        public PidStatusDto Status { get; private set; }

        private double target = 0;

        private readonly PIDRegulator2 _pidRegulator;
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
                        PIDKp = PIDKp,
                        PIDKi = PIDKi,
                        PIDKd = PIDKd
                    };
                    db.Add(PidConfig);
                    db.SaveChanges();
                }
            }
            _pidRegulator = new PIDRegulator2(PidConfig.PIDKp, PidConfig.PIDKi, PidConfig.PIDKd, 100, 0, 100, 0);
            Status = new PidStatusDto
            {
                PidId = _pidId,
                PidName = _pidName,
            };

        }

        public void Calculate(TempReaderResultDto currentTempResult)
        {
            var currentTemp = _pidId == 0 ? currentTempResult.Temp1 : currentTempResult.Temp2;
            var outputValue = _pidRegulator.Compute(currentTemp, Status.TargetTemp);
            _heater.UpdateNextCyclePercentage(outputValue);

            Status.CurrentTemp = currentTemp;
            Status.OutputValue = outputValue;
            Status.Output = _heater.CurrentStatus;

            //_gPIO.Set(_outPin, Status.Output);
        }

        public void UpdateTargetTemp(double newTargetTemp)
        {
            this.target = newTargetTemp;
            Status.TargetTemp = newTargetTemp;
            _pidRegulator.Reset();
        }

    }
}