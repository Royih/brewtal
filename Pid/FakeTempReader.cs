using System;
using System.Linq;
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
        private double temparature2 = startTemp;
        private DateTime _lastRead1 = DateTime.Now;
        private DateTime _lastRead2 = DateTime.Now;
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
                var secondsSinceLastRead = DateTime.Now.Subtract(pidId == 0 ? _lastRead1 : _lastRead2).TotalSeconds;
                var percentage = worker.Status.Single(x => x.PidId == pidId).OutputValue;
                double increase = ((secondsSinceLastRead * degreesRisesPerSecondAtMaxPercentage * percentage) / 100) - degreesLostPerSecond;

                if (pidId == 0)
                {
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
                temparature2 += increase;
                if (temparature2 > 100)
                {
                    temparature2 = 100;
                }
                if (temparature2 < startTemp)
                {
                    temparature2 = startTemp;
                }
                _lastRead2 = DateTime.Now;
                return temparature2;
            }
        }

        public TempReaderResultDto ReadTemp()
        {
            return new TempReaderResultDto
            {
                Temp1 = ReadTemp(0),
                Temp2 = ReadTemp(1)
            };
        }
    }

}
