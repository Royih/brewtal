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

        private DateTime _previousComputeTime = DateTime.Now.AddSeconds(-1);

        public PidSatusDto Status { get; private set; }

        private double target = 0;

        private readonly PIDRegulator _pidRegulator;
        private readonly IGPIO _gPIO;
        private readonly int _outPin;
        private readonly HeaterController _heater;


        public PID(int pidId, string pidName, IGPIO gPIO, int outPin)
        {
            _pidId = pidId;
            _pidName = pidName;
            _gPIO = gPIO;
            _outPin = outPin;
            _heater = new HeaterController(gPIO, outPin);
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
            _pidRegulator = new PIDRegulator(PidConfig.PIDKp, PidConfig.PIDKi, PidConfig.PIDKd, 100, 0, 100, 0);
            Status = new PidSatusDto
            {
                PidId = _pidId,
                PidName = _pidName,
            };
            
        }

        public void Calculate(TempReaderResultDto currentTempResult)
        {
            var currentTemp = _pidId == 0 ? currentTempResult.Temp1 : currentTempResult.Temp2;
            var outputValue = _pidRegulator.Compute(currentTemp, Status.TargetTemp, DateTime.Now.Subtract(_previousComputeTime));
            _heater.UpdateNextCyclePercentage(outputValue);
            _previousComputeTime = DateTime.Now;            

            Status.CurrentTemp = currentTemp;
            Status.OutputValue = outputValue;
            Status.Output = _heater.CurrentStatus;
            Status.ErrorSum = _pidRegulator.ErrorSum;

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