using System;
using System.Linq;
using System.Threading.Tasks;
using Brewtal2.DataAccess;
using Brewtal2.Infrastructure.SignalR;
using Brewtal2.Pid.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using Serilog;

namespace Brewtal2.Pid
{
    public class BackgroundWorker
    {
        private readonly ITempReader _tempReader;
        private readonly IHubContext<ComHub> _hubContext;
        private readonly BrewIO _brewIO;
        private IPidRepository _pidRepo;
        private PID _pid0;
        private PID _pid1;

        public PidStatusDto[] Status
        {
            get
            {
                return new [] { _pid0.Status, _pid1.Status };
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
            if (pidId == 0)
            {
                _pid0.UpdateTargetTemp(newTargetTemp);
                Log.Debug($"Updated PID Target 1:{_pid0.Status.TargetTemp}");
            }
            else
            {
                _pid1.UpdateTargetTemp(newTargetTemp);
                Log.Debug($"Updated PID Target 2:{_pid1.Status.TargetTemp}");
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

            var pidConfig = _pidRepo.GetPidConfig(pidId);
            pidConfig.PIDKp = newPIDKp;
            pidConfig.PIDKi = newPIDKi;
            pidConfig.PIDKd = newPIDKd;
            _pidRepo.UpdateExistingPidConfig(pidConfig);

            if (pidId == 0)
            {
                _pid0 = new PID(0, "Pid 1", _brewIO, Outputs.Pid1Output, _pidRepo);
            }
            else
            {
                _pid1 = new PID(1, "Pid 2", _brewIO, Outputs.Pid2Output, _pidRepo);
            }
        }

        public async void Start(IPidRepository pidRepo)
        {
            _pidRepo = pidRepo;
            Log.Debug($"{DateTime.Now}: Starting Worker");
            _pid0 = new PID(0, "Pid 1", _brewIO, Outputs.Pid1Output, _pidRepo);
            _pid1 = new PID(1, "Pid 2", _brewIO, Outputs.Pid2Output, _pidRepo);
            while (true)
            {
                await Task.Run(() =>
                {

                    var newTemp = _tempReader.ReadTemp();
                    _pid0.Calculate(newTemp);
                    _pid1.Calculate(newTemp);

                    _hubContext.Clients.All.SendAsync("HarwareStatus", new HardwareStatusDto
                    {
                        Pids = new [] { _pid0.Status, _pid1.Status }.ToArray(),
                            ComputedTime = DateTime.Now,
                            ManualOutputs = _brewIO.SupportedOutputs.Where(x => !x.Automatic).ToArray()
                    });

                    System.Threading.Thread.Sleep(500);

                });
            }
        }

    }

}