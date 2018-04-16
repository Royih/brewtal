using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Brewtal.Dtos;

namespace Brewtal.BLL
{
    public class PidWorker
    {

        private readonly ILogger<Worker> _logger;
        private readonly ITempReader _tempReader;
        private readonly IHubContext<TempHub> _hubContext;
        private readonly IGPIO _gPIO;

        private PID[] _pids;

        public PidWorker(ILogger<Worker> logger, ITempReader tempReader, IHubContext<TempHub> hubContext, IGPIO gPIO)
        {
            _logger = logger;
            _tempReader = tempReader;
            _hubContext = hubContext;
            _gPIO = gPIO;
            _pids = new[] {
                 new PID(0, "Pid 1", _gPIO, 4),
                 new PID(1, "Pid 2", _gPIO, 26)
            };
        }


        public void UpdateTargetTemp(int pidId, double newTargetTemp)
        {
            var pid = _pids.Single(x => x.Status.PidId == pidId);
            pid.UpdateTargetTemp(newTargetTemp);
        }


        public async void Start()
        {
            _logger.LogDebug($"{DateTime.Now}: Starting Pid Worker");
            while (true)
            {
                //Task running not so often that updates PID-Output
                await Task.Run(() =>
                {
                    var newTemp = _tempReader.ReadTemp();
                    foreach (var pid in _pids)
                    {
                        pid.Calculate(newTemp);
                    }
                    _logger.LogDebug(20, $"Reporting Current PID Status for {_pids.Count()} pids.");
                    _hubContext.Clients.All.InvokeAsync("PIDUpdate", new PidStatusesDto { Pids = _pids.Select(x => x.Status).ToArray() });
                    System.Threading.Thread.Sleep(1000);
                });
            }
        }



    }

}