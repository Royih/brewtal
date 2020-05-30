using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Brewtal2.Pid
{
    public class GPIO : IGPIO
    {
        private readonly ILogger<GPIO> _logger;

        public GPIO(ILogger<GPIO> logger)
        {
            _logger = logger;
        }

        public bool Get(int pinId)
        {
            var pin = Pi.Gpio[pinId];
            pin.PinMode = GpioPinDriveMode.Output;
            return pin.Read();
        }

        public bool Set(int pinId, bool status)
        {
            var pin = Pi.Gpio[pinId];
            pin.PinMode = GpioPinDriveMode.Output;
            pin.Write(status);
            //Pi.Spi.Channel0Frequency = SpiChannel.MinFrequency;
            return pin.Read();
        }
    }

}