using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Brewtal.Dtos;
using Brewtal.Database;

namespace Brewtal.BLL
{
    public class BackgroundWorker
    {

        private readonly ILogger<BackgroundWorker> _logger;
        private readonly ITempReader _tempReader;
        private readonly IHubContext<BrewtalHub> _hubContext;
        private readonly BrewIO _brewIO;
        private PID _pid0;
        private PID _pid1;

        public PidStatusDto[] Status
        {
            get
            {
                return new[] { _pid0.Status, _pid1.Status };
            }
        }

        public BackgroundWorker(ILogger<BackgroundWorker> logger, ITempReader tempReader, IHubContext<BrewtalHub> hubContext, BrewIO brewIO)
        {
            _logger = logger;
            _tempReader = tempReader;
            _hubContext = hubContext;
            _brewIO = brewIO;
            _pid0 = new PID(0, "Pid 1", _brewIO, Outputs.Pid1Output);
            _pid1 = new PID(1, "Pid 2", _brewIO, Outputs.Pid2Output);
        }


        public void UpdateTargetTemp(int pidId, double newTargetTemp)
        {
            if (pidId == 0)
            {
                _pid0.UpdateTargetTemp(newTargetTemp);
                _logger.LogDebug(20, $"Updated PID Target 1:{_pid0.Status.TargetTemp}");
            }
            else
            {
                _pid1.UpdateTargetTemp(newTargetTemp);
                _logger.LogDebug(20, $"Updated PID Target 2:{_pid1.Status.TargetTemp}");
            }
        }

        public PidConfig GetConfig(int pidId)
        {
            if (pidId == 0)
            {
                return _pid0.PidConfig;
            }
            return _pid1.PidConfig;
        }

        public void UpdatePidConfig(int pidId, double newPIDKp, double newPIDKi, double newPIDKd)
        {
            using (var db = new BrewtalContext())
            {
                var pidConfig = db.PidConfigs.Single(x => x.PidId == pidId);
                pidConfig.PIDKp = newPIDKp;
                pidConfig.PIDKi = newPIDKi;
                pidConfig.PIDKd = newPIDKd;
                db.SaveChanges();
            }
            if (pidId == 0)
            {
                _pid0 = new PID(0, "Pid 1", _brewIO, Outputs.Pid1Output);
            }
            else
            {
                _pid1 = new PID(1, "Pid 2", _brewIO, Outputs.Pid2Output);
            }
        }

        public async void Start()
        {
            _logger.LogDebug($"{DateTime.Now}: Starting Worker");
            while (true)
            {
                await Task.Run(() =>
                {

                    var newTemp = _tempReader.ReadTemp();
                    _pid0.Calculate(newTemp);
                    _pid1.Calculate(newTemp);

                    _hubContext.Clients.All.InvokeAsync("HarwareStatus", new HardwareStatusDto
                    {
                        Pids = new[] { _pid0.Status, _pid1.Status }.ToArray(),
                        ComputedTime = DateTime.Now,
                        ManualOutputs = _brewIO.SupportedOutputs.Where(x => !x.Automatic).ToArray()
                    });

                    System.Threading.Thread.Sleep(500);
                });
            }
        }

    }

}