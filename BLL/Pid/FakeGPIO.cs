
using Microsoft.Extensions.Logging;

namespace Brewtal2.BLL.Pid
{
    public class FakeGPIO : IGPIO
    {
        private readonly ILogger<FakeGPIO> _logger;

        public FakeGPIO(ILogger<FakeGPIO> logger)
        {
            _logger = logger;
        }

        public bool Get(int pinId)
        {
            return false;
        }

        public bool Set(int pinId, bool status)
        {
            return status;
        }
    }

}
