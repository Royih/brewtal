


using Brewtal2.Pid.Models;

namespace Brewtal2.Pid
{
    public interface ITempReader
    {
        TempReaderResultDto ReadTemp();
    }

}