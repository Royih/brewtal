using System;
using System.Threading;
using Brewtal.Dtos;
using Microsoft.Extensions.Logging;

namespace Brewtal.BLL
{
    public class PID
    {
        private readonly int _pidId;
        private readonly string _pidName;
        public const double PIDKp = 1.0f; // Increase if slow or not reaching set value

        public const double PIDKi = 2.0f; // Decrease to avoid overshoot

        public const double PIDKd = 3.0f; /*1.0 gave major overshoot */

        private DateTime _previousComputeTime = DateTime.Now.AddSeconds(-1);

        public PidSatusDto Status { get; private set; }

        private double target = 0;


        private readonly PIDRegulator _pidRegulator;
        private readonly IGPIO _gPIO;
        private readonly int _outPin;


        public PID(int pidId, string pidName, IGPIO gPIO, int outPin)
        {
            _pidId = pidId;
            _pidName = pidName;
            _gPIO = gPIO;
            _outPin = outPin;
            _pidRegulator = new PIDRegulator(PIDKp, PIDKi, PIDKd, 100, 0, 1, 0);
            Status = new PidSatusDto
            {
                PidId = _pidId,
                PidName = _pidName,
                TargetTemp = 15.0,
                CurrentTemp = 0,
                Output = false
            };

        }

        public void Calculate(TempReaderResultDto currentTempResult)
        {
            var currentTemp = _pidId == 0 ? currentTempResult.Temp1 : currentTempResult.Temp2;
            var output = _pidRegulator.Compute(currentTemp, Status.TargetTemp, DateTime.Now.Subtract(_previousComputeTime));
            _previousComputeTime = DateTime.Now;

            Status.CurrentTemp = currentTemp;
            Status.Output = output;

            _gPIO.Set(_outPin, output);
        }
        public void UpdateTargetTemp(double newTargetTemp)
        {
            this.target = newTargetTemp;
            Status.TargetTemp = newTargetTemp;
            _pidRegulator.Reset();
        }
    }
}