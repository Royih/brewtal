
using Brewtal2.Models.Brews;


namespace Brewtal2.BLL.Pid
{
    public interface ITempReader
    {
        TempReaderResultDto ReadTemp();
    }

}