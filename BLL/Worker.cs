using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Brewtal.BLL
{
    public class Worker
    {

        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<TempHub> _hubContext;

        public Worker(ILogger<Worker> logger, IHubContext<TempHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async void Start(ITempReader tempReader)
        {
            _logger.LogDebug($"{DateTime.Now}: Starting Worker");
            while (true)
            {
                await Task.Run(() =>
                {
                    var newTemp = tempReader.ReadTemp();
                    _hubContext.Clients.All.InvokeAsync("TempUpdate", newTemp);
                    _logger.LogDebug(20, "Emitting new temp {0}", newTemp);
                    System.Threading.Thread.Sleep(300);
                });
            }
        }

    }

}