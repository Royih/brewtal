using System;
using System.Linq;
using System.Threading.Tasks;
using Brewtal2.Infrastructure.SignalR;
using Brewtal2.Pid.Models;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Brewtal2.Pid
{
    public class BackgroundWorker
    {
        private readonly ITempReader _tempReader;
        private readonly IHubContext<ComHub> _hubContext;
        private readonly BrewIO _brewIO;
        private IPidRepository _pidRepo;
        private PID _pid;

        public PidStatusDto Status
        {
            get
            {
                return _pid.Status;
            }
        }

        public BackgroundWorker(ITempReader tempReader, IHubContext<ComHub> hubContext, BrewIO brewIO)
        {
            _tempReader = tempReader;
            _hubContext = hubContext;
            _brewIO = brewIO;
        }

        public void UpdateTargetTemp(int pidId, double newTargetTemp)
        {
            _pid.UpdateTargetTemp(newTargetTemp);
            Log.Debug($"Updated PID Target 1:{_pid.Status.TargetTemp}");
        }

        public PidConfig GetConfig()
        {
            return _pid.PidConfig;
        }

        public void UpdatePidConfig(int pidId, double newPIDKp, double newPIDKi, double newPIDKd)
        {

            var pidConfig = _pidRepo.GetPidConfig();
            pidConfig.PIDKp = newPIDKp;
            pidConfig.PIDKi = newPIDKi;
            pidConfig.PIDKd = newPIDKd;
            _pidRepo.UpdateExistingPidConfig(pidConfig);
            _pid = new PID("Pid 1", _brewIO, Outputs.Pid1Output, _pidRepo);
        }

        public async void Start(IPidRepository pidRepo)
        {
            _pidRepo = pidRepo;
            Log.Debug($"{DateTime.Now}: Starting Worker");
            _pid = new PID("Pid 1", _brewIO, Outputs.Pid1Output, _pidRepo);
            while (true)
            {
                await Task.Run(() =>
                {
                    var newTemp = _tempReader.ReadTemp();
                    _pid.Calculate(newTemp);

                    _hubContext.Clients.All.SendAsync("HarwareStatus", new HardwareStatusDto
                    {
                        Pids = new[] { _pid.Status }.ToArray(),
                        ComputedTime = DateTime.Now,
                        ManualOutputs = _brewIO.SupportedOutputs.Where(x => !x.Automatic).ToArray()
                    });
                    System.Threading.Thread.Sleep(500);
                });
            }
        }

    }

}