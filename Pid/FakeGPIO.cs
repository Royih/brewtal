using Microsoft.Extensions.Logging;

namespace Brewtal2.Pid
{
    public class FakeGPIO : IGPIO
    {

        private bool[] _values;
        private readonly ILogger<FakeGPIO> _logger;

        public FakeGPIO(ILogger<FakeGPIO> logger)
        {
            _logger = logger;
            _values = new bool[100];
        }

        public bool Get(int pinId)
        {
            if (pinId < _values.Length)
            {
                return _values[pinId];
            }
            return false;
        }

        public bool Set(int pinId, bool status)
        {
            if (pinId < _values.Length)
            {
                _values[pinId] = status;
            }
            return status;
        }
    }

}