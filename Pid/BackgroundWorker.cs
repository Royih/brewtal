using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brewtal2.Infrastructure.SignalR;
using Brewtal2.Pid.Models;
using Brewtal2.Storage;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Brewtal2.Pid
{
    public class BackgroundWorker
    {
        private readonly ITempReader _tempReader;
        private readonly IHubContext<ComHub> _hubContext;
        private readonly IStorageRepository _storageRepository;
        private readonly IMapper _mapper;
        private readonly BrewIO _brewIO;
        private IPidRepository _pidRepo;
        private PID _pid;

        private readonly TimeSpan TempLogSampleRate = TimeSpan.FromMinutes(1);

        public PidStatusDto Status
        {
            get
            {
                return _pid.Status;
            }
        }
        private Session _currentSession;
        private DateTime? _nextTempLog = null;

        public BackgroundWorker(ITempReader tempReader, IHubContext<ComHub> hubContext, IStorageRepository storageRepository, BrewIO brewIO, IMapper mapper)
        {
            _tempReader = tempReader;
            _hubContext = hubContext;
            _storageRepository = storageRepository;
            _currentSession = storageRepository.GetCurrentSession();
            _brewIO = brewIO;
            _mapper = mapper;
        }

        public void UpdateTargetTemp(double newTargetTemp)
        {
            _pid.UpdateTargetTemp(newTargetTemp);
            _currentSession = _storageRepository.SetNewTargetTemp(newTargetTemp, _pid.Status.CurrentTemp);
            _nextTempLog = null;
            Log.Debug($"Updated PID Target 1:{_pid.Status.TargetTemp}");
        }

        public void SetFridgeMode()
        {
            _pid.UpdatePIDMode(true);
            Log.Debug($"Changed to Fridge mode");
        }

        public void SetPidMode()
        {
            _pid.UpdatePIDMode(false);
            Log.Debug($"Changed to PID mode");
        }

        public PidConfig GetConfig()
        {
            return _pid.PidConfig;
        }

        public void UpdatePidConfig(int pidId, double newPIDKp, double newPIDKi, double newPIDKd)
        {

            var pidConfig = _pidRepo.GetPidConfig();
            pidConfig.PIDKp = newPIDKp;
            pidConfig.PIDKi = newPIDKi;
            pidConfig.PIDKd = newPIDKd;
            _pidRepo.UpdateExistingPidConfig(pidConfig);
            _pid = new PID("Pid 1", _brewIO, Outputs.Pid1Output, _pidRepo, _currentSession?.TargetTemp ?? 0);
        }

        public async void Start(IPidRepository pidRepo)
        {
            _pidRepo = pidRepo;

            Log.Debug($"Starting Worker");
            _pid = new PID("Pid 1", _brewIO, Outputs.Pid1Output, _pidRepo, _currentSession?.TargetTemp ?? 0);
            while (true)
            {
                await Task.Run(() =>
                {
                    var newTemp = _tempReader.ReadTemp();
                    _pid.Calculate(newTemp);

                    _hubContext.Clients.All.SendAsync("HarwareStatus", new HardwareStatusDto
                    {
                        Pid = _pid.Status,
                        ComputedTime = DateTime.Now,
                        ManualOutputs = _brewIO.SupportedOutputs.Where(x => !x.Automatic).ToArray(),
                        PidConfig = _pidRepo.GetPidConfig()
                    });


                    if (_currentSession != null && (!_nextTempLog.HasValue || DateTime.Now > _nextTempLog.Value))
                    {
                        var newLog = _storageRepository.LogTemp(newTemp.Temp1);
                        var newLogDto = _mapper.Map<TemplogDto>(newLog);
                        _hubContext.Clients.All.SendAsync("NewTempLog", newLogDto);
                        Log.Debug($"Temperature {newTemp.Temp1}°C logged");
                        _nextTempLog = DateTime.Now.Add(TempLogSampleRate);
                    }
                    System.Threading.Thread.Sleep(500);
                });
            }
        }

    }

}