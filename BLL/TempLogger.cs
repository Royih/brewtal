using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Brewtal.Dtos;
using Brewtal.Database;

namespace Brewtal.BLL
{
    public class TempLogger
    {

        private readonly ILogger<TempLogger> _logger;
        private readonly BackgroundWorker _worker;

        public TempLogger(ILogger<TempLogger> logger, BackgroundWorker worker)
        {
            _logger = logger;
            _worker = worker;
        }
        public async void Start()
        {
            _logger.LogDebug($"{DateTime.Now}: Starting Temp logger");
            while (true)
            {
                await Task.Run(() =>
                {
                    LogToDb();
                    System.Threading.Thread.Sleep(15000);
                });
            }
        }

        private void LogToDb()
        {
            var statuses = _worker.Status;
            using (var db = new BrewtalContext())
            {
                var loggingSession = db.Sessions.Where(x => !x.Completed.HasValue).SingleOrDefault();
                if (loggingSession != null)
                {
                    var pid1Status = statuses[0];
                    var pid2Status = statuses[1];
                    var logRecord = new LogRecord
                    {
                        Session = loggingSession,
                        TimeStamp = DateTime.Now,
                        ActualTemp1 = pid1Status.CurrentTemp,
                        TargetTemp1 = pid1Status.TargetTemp,
                        Output1 = pid1Status.Output,
                        ActualTemp2 = pid2Status.CurrentTemp,
                        TargetTemp2 = pid2Status.TargetTemp,
                        Output2 = pid2Status.Output
                    };
                    db.Add(logRecord);
                    db.SaveChanges();
                }
            }
        }

    }

}