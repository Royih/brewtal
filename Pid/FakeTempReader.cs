using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Brewtal2.Pid.Models;

namespace Brewtal2.Pid
{
    public class FakeTempReader : ITempReader
    {
        private readonly ILogger<FakeTempReader> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const double startTemp = 10.0;
        private double temparature1 = startTemp;
        private DateTime _lastRead1 = DateTime.Now;
        const double degreesRisesPerSecondAtMaxPercentage = 0.1;
        const double degreesLostPerSecond = 0.01;

        public FakeTempReader(ILogger<FakeTempReader> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private double ReadTemp(int pidId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var worker = scope.ServiceProvider.GetRequiredService<BackgroundWorker>();
                var secondsSinceLastRead = DateTime.Now.Subtract(_lastRead1).TotalSeconds;
                var percentage = worker.Status.OutputValue;
                double increase = ((secondsSinceLastRead * degreesRisesPerSecondAtMaxPercentage * percentage) / 100) - degreesLostPerSecond;

                temparature1 += increase;
                if (temparature1 > 100)
                {
                    temparature1 = 100;
                }
                if (temparature1 < startTemp)
                {
                    temparature1 = startTemp;
                }
                _lastRead1 = DateTime.Now;
                return temparature1;

            }
        }

        public TempReaderResultDto ReadTemp()
        {
            return new TempReaderResultDto
            {
                Temp1 = ReadTemp(0),
            };
        }
    }

}
