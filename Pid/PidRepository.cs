using System;
using Brewtal2.Pid.Models;

namespace Brewtal2.Pid
{
    public class PidRepository : IPidRepository
    {
        public void AddPidConfig(PidConfig pidConfig)
        {
            throw new NotImplementedException();
        }

        public PidConfig GetPidConfig()
        {
            return new PidConfig { PIDKi = 0.12, PIDKd = 0, PIDKp = 48.6 };
        }

        public void UpdateExistingPidConfig(PidConfig pidConfig)
        {
            throw new NotImplementedException();
        }
    }
}