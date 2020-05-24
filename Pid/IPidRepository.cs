using Brewtal2.Pid.Models;

namespace Brewtal2.Pid
{
    public interface IPidRepository
    {
        PidConfig GetPidConfig(int pidId);
        void AddPidConfig(PidConfig pidConfig);
        void UpdateExistingPidConfig(PidConfig pidConfig);
    }
}