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

        private readonly PIDRegulator3 _pidRegulator;
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
                PidName = _pidName
            };

        }

        public void Calculate(TempReaderResultDto currentTempResult)
        {
            var currentTemp = currentTempResult.Temp1;
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