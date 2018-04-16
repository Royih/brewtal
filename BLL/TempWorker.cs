using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Brewtal.BLL
{
    public class TempWorker
    {

        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<TempHub> _hubContext;
        private readonly ITempReader _tempReader;

        public TempWorker(ILogger<Worker> logger, ITempReader tempReader, IHubContext<TempHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            _tempReader = tempReader;
        }

        public async void Start()
        {

            _logger.LogDebug($"{DateTime.Now}: Starting Temp Worker");
            while (true)
            {
                await Task.Run(() =>
                {
                    var currentTemp = _tempReader.ReadTemp();

                    _hubContext.Clients.All.InvokeAsync("TempUpdate", currentTemp);
                    _logger.LogDebug(20, $"Emitting new temp: {currentTemp}");

                    System.Threading.Thread.Sleep(500);
                });
            }
        }

    }

}