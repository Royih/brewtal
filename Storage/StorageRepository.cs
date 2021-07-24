using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Brewtal2.Storage
{
    public class StorageRepository : IStorageRepository
    {
        private readonly StorageContext _db;
        public StorageRepository()
        {
            _db = new StorageContext();
        }

        public Session GetCurrentSession()
        {
            var runtime = _db.Runtime.Include(x => x.CurrentSession).Single();
            return runtime.CurrentSession;
        }

        public void InitializeDb()
        {
            _db.Add(new Runtime() { Startup = DateTime.Now });
            _db.SaveChanges();
            Log.Information("DB initialized");
        }

        public void LogTemp(double newTemp)
        {
            var runtime = _db.Runtime.Include(x => x.CurrentSession).Single();
            var currentSession = runtime.CurrentSession;
            _db.Add(new Templog
            {
                TimeStamp = DateTime.Now,
                ActualTemperature = newTemp,
                SessionId = currentSession.Id
            });
            if (newTemp < currentSession.MinTemp)
            {
                currentSession.MinTemp = newTemp;
            }
            if (newTemp > currentSession.MaxTemp)
            {
                currentSession.MaxTemp = newTemp;
            }
            if (!currentSession.TimeToReachTarget.HasValue && Math.Abs(currentSession.TargetTemp - newTemp) < 1)
            {
                currentSession.TimeToReachTarget = DateTime.Now.Subtract(currentSession.StartTime);
            }
            _db.SaveChanges();
        }

        public void RegisterStartup()
        {
            var runtime = _db.Runtime.Include(x => x.CurrentSession).Single();
            runtime.Startup = DateTime.Now;
            if (runtime.CurrentSession != null && runtime.CurrentSession.StartTime.Date != DateTime.Now.Date)
            {
                //End previous Temp-Log-Session unless from today
                runtime.CurrentSession = null;
                Log.Information("Ending ongoing session");
            }
            else if (runtime.CurrentSession != null)
            {
                Log.Information("Resuming ongoing session");
            }
            _db.SaveChanges();
        }

        public Session SetNewTargetTemp(double newTargetTemp, double actualTemp)
        {
            var runtime = _db.Runtime.Include(x => x.CurrentSession).Single();

            if (newTargetTemp > 0 && (runtime.CurrentSession == null || runtime.CurrentSession.TargetTemp != newTargetTemp))
            {
                if (runtime.CurrentSession != null)
                {
                    Log.Information($"{DateTime.Now}: Ended current Temp-Log-Session");
                }
                var newSession = new Session
                {
                    StartTime = DateTime.Now,
                    TargetTemp = newTargetTemp,
                    MinTemp = actualTemp,
                    MaxTemp = actualTemp
                };
                _db.Sessions.Add(newSession);
                runtime.CurrentSession = newSession;
                _db.SaveChanges();

                Log.Information($"{DateTime.Now}: New Temp-Log-Session started");
                return newSession;
            }
            else if (newTargetTemp == 0)
            {
                runtime.CurrentSession = null;
                Log.Information($"{DateTime.Now}: Ended current Temp-Log-Session");
                _db.SaveChanges();
                return null;
            }

            return runtime.CurrentSession;

        }
    }
}