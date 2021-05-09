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
            return new PidConfig { PIDKi = 200, PIDKd = 10, PIDKp = 200 };
        }

        public void UpdateExistingPidConfig(PidConfig pidConfig)
        {
            throw new NotImplementedException();
        }
    }
}