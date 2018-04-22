using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Brewtal.Dtos;
using Brewtal.Database;

namespace Brewtal.BLL
{
    public class PidWorker
    {

        private readonly ILogger<PidWorker> _logger;
        private readonly ITempReader _tempReader;
        private readonly IHubContext<BrewtalHub> _hubContext;
        private readonly IGPIO _gPIO;

        private const int Pid1OutputPin = 4;
        private const int Pid2OutputPin = 26;
        private PID _pid0;
        private PID _pid1;

        private readonly IServiceProvider _serviceProvider;

        public PidWorker(ILogger<PidWorker> logger, ITempReader tempReader, IHubContext<BrewtalHub> hubContext, IGPIO gPIO, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _tempReader = tempReader;
            _hubContext = hubContext;
            _gPIO = gPIO;
            _serviceProvider = serviceProvider;
            _pid0 = new PID(0, "Pid 1", _gPIO, Pid1OutputPin);
            _pid1 = new PID(1, "Pid 2", _gPIO, Pid2OutputPin);
        }


        public void UpdateTargetTemp(int pidId, double newTargetTemp)
        {
            if (pidId == 0)
            {
                _pid0.UpdateTargetTemp(newTargetTemp);
                _logger.LogDebug(20, $"Updated PID Target 1:{_pid0.Status.TargetTemp}");
            }
            else
            {
                _pid1.UpdateTargetTemp(newTargetTemp);
                _logger.LogDebug(20, $"Updated PID Target 2:{_pid1.Status.TargetTemp}");
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
            using (var db = new BrewtalContext())
            {
                var pidConfig = db.PidConfigs.Single(x => x.PidId == pidId);
                pidConfig.PIDKp = newPIDKp;
                pidConfig.PIDKi = newPIDKi;
                pidConfig.PIDKd = newPIDKd;
                db.SaveChanges();
            }
            if (pidId == 0)
            {
                _pid0 = new PID(0, "Pid 1", _gPIO, Pid1OutputPin);
            }
            else
            {
                _pid1 = new PID(1, "Pid 2", _gPIO, Pid2OutputPin);
            }
        }

        public async void Start()
        {
            _logger.LogDebug($"{DateTime.Now}: Starting Pid Worker");
            while (true)
            {
                _logger.LogDebug(20, $"Reporting Current PID Status for pids. 1:{_pid0.Status.TargetTemp}, 2: {_pid1.Status.TargetTemp}");
                //Task running not so often that updates PID-Output
                await Task.Run(() =>
                {

                    var newTemp = _tempReader.ReadTemp();
                    _pid0.Calculate(newTemp);
                    _pid1.Calculate(newTemp);

                    var loggingSession = LogToDb();

                    _hubContext.Clients.All.InvokeAsync("PIDUpdate", new PidStatusesDto
                    {
                        Pids = new[] { _pid0.Status, _pid1.Status }.ToArray(),
                        ComputedTime = DateTime.Now,
                        LoggingToName = loggingSession?.Name
                    });

                    System.Threading.Thread.Sleep(1000);
                });
            }
        }

        private LogSession LogToDb()
        {
            using (var db = new BrewtalContext())
            {
                var loggingSession = db.Sessions.Where(x => !x.Completed.HasValue).SingleOrDefault();

                if (loggingSession != null)
                {
                    var pid1Status = _pid0.Status;
                    var pid2Status = _pid1.Status;
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
                    return loggingSession;
                }
            }
            return null;
        }



    }

}