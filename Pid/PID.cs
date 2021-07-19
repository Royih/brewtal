using System;
using Brewtal2.Infrastructure;
using Brewtal2.Pid.Models;
namespace Brewtal2.Pid
{
    public class PID
    {
        private readonly string _pidName;
        public readonly PidConfig PidConfig;
        private const double DefaultPIDKp = 200;

        private const double DefaultPIDKi = 200;

        private const double DefaultPIDKd = 10;

        public PidStatusDto Status { get; private set; }

        private double target = 0;

        private IPIDRegulator _pidRegulator;
        private readonly BrewIO _brewIO;
        private readonly Outputs _output;
        private readonly HeaterController _heater;

        public PID(string pidName, BrewIO brewIO, Outputs output, IPidRepository pidRepo)
        {
            _pidName = pidName;
            _brewIO = brewIO;
            _output = output;
            _heater = new HeaterController(_brewIO, output);
            _heater.Start();

            PidConfig = pidRepo.GetPidConfig();
            if (PidConfig == null)
            {
                PidConfig = new PidConfig
                {
                    PIDKp = DefaultPIDKp,
                    PIDKi = DefaultPIDKi,
                    PIDKd = DefaultPIDKd
                };
                pidRepo.AddPidConfig(PidConfig);
            }

            _pidRegulator = new PIDRegulator3(PidConfig.PIDKp, PidConfig.PIDKi, PidConfig.PIDKd);
            Status = new PidStatusDto
            {
                PidName = _pidName,
            };

        }

        public void Calculate(TempReaderResultDto currentTempResult)
        {
            var currentTemp = currentTempResult.Temp1;
            var outputValue = _pidRegulator.Compute(currentTemp, Status.TargetTemp);
            _heater.UpdateNextCyclePercentage(outputValue);

            Status.CurrentTemp = currentTemp;
            Status.OutputValue = outputValue;
            Status.Output = _heater.CurrentStatus;
            Status.ErrorSum = _pidRegulator.ErrorSum;
            
            if (!Status.MaxTemp.HasValue || currentTemp > Status.MaxTemp)
            {
                Status.MaxTemp = currentTemp;
                Status.MaxTempTimeStamp = DateTime.Now;
            }
            if (!Status.MinTemp.HasValue || currentTemp < Status.MinTemp)
            {
                Status.MinTemp = currentTemp;
                Status.MinTempTimeStamp = DateTime.Now;
            }

            Status.RPICoreTemp = "vcgencmd measure_temp".Bash();

        }

        public void UpdateTargetTemp(double newTargetTemp)
        {
            this.target = newTargetTemp;
            Status.TargetTemp = newTargetTemp;
            _pidRegulator.Reset();
            Status.MaxTemp = null;
            Status.MinTemp = null;
            Status.MaxTempTimeStamp = null;
            Status.MinTempTimeStamp = null;
        }

        public void UpdatePIDMode(bool fridgeMode)
        {
            if (fridgeMode)
            {
                _pidRegulator = new PIDRegulatorFridge();
                Status.FridgeMode = true;
            }
            else
            {
                _pidRegulator = new PIDRegulator3(PidConfig.PIDKp, PidConfig.PIDKi, PidConfig.PIDKd);
                Status.FridgeMode = false;
            }
            _pidRegulator.Reset();
            Status.MaxTemp = null;
            Status.MinTemp = null;
            Status.MaxTempTimeStamp = null;
            Status.MinTempTimeStamp = null;
        }

    }
}