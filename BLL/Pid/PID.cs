using System.Linq;
using Brewtal2.DataAccess;
using Brewtal2.Models.Brews;
using MongoDB.Driver;

namespace Brewtal2.BLL.Pid
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

        public PID(int pidId, string pidName, BrewIO brewIO, Outputs output, IDb db)
        {
            _pidId = pidId;
            _pidName = pidName;
            _brewIO = brewIO;
            _output = output;
            _heater = new HeaterController(_brewIO, output);
            _heater.Start();

            PidConfig = db.PidConfigs.Find(x => x.PidId == pidId).SingleOrDefault();
            if (PidConfig == null)
            {
                PidConfig = new PidConfig
                {
                PidId = pidId,
                PIDKp = DefaultPIDKp,
                PIDKi = DefaultPIDKi,
                PIDKd = DefaultPIDKd
                };
                db.PidConfigs.InsertOne(PidConfig);
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