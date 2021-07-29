using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Brewtal2.Pid.Models;
using Brewtal2.Storage;

namespace Brewtal2.Pid
{
    public class FakeTempReader : ITempReader
    {
        private readonly ILogger<FakeTempReader> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IStorageRepository _storageRepository;

        private const double startTemp = 10.0;
        private double temparature1 = startTemp;
        private DateTime _lastRead1 = DateTime.Now;
        const double degreesRisesPerSecondAtMaxPercentage = 0.1;
        const double degreesLostPerSecond = 0.01;

        public FakeTempReader(ILogger<FakeTempReader> logger, IServiceScopeFactory serviceScopeFactory, IStorageRepository storageRepository)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _storageRepository = storageRepository;
            var currentSession = _storageRepository.GetCurrentSession();

            // If current session is ongoing, then the fake temperature should start at where it was at last logged temperature. 
            // This is to simulate real life.
            if (currentSession != null)
            {
                var latestTempLog = _storageRepository.GetLatestTempLog(currentSession.Id);
                if (latestTempLog != null)
                {
                    temparature1 = latestTempLog.ActualTemperature;
                }
            }
        }

        private double ReadTemp(int pidId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var worker = scope.ServiceProvider.GetRequiredService<BackgroundWorker>();
                var secondsSinceLastRead = DateTime.Now.Subtract(_lastRead1).TotalSeconds;
                var percentage = worker.Status.OutputValue;
                double increase = ((secondsSinceLastRead * degreesRisesPerSecondAtMaxPercentage * percentage) / 100) - degreesLostPerSecond;
                if (worker.Status.FridgeMode)
                {
                    increase = (-1) * increase;
                }

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
