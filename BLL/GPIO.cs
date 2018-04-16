using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace Brewtal.BLL
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
            var pin = Pi.Gpio.GetGpioPinByBcmPinNumber(pinId);
            pin.PinMode = GpioPinDriveMode.Output;
            return pin.Read();
        }

        public bool Set(int pinId, bool status)
        {
            var pin = Pi.Gpio.GetGpioPinByBcmPinNumber(pinId);
            pin.PinMode = GpioPinDriveMode.Output;
            pin.Write(status);
            //Pi.Spi.Channel0Frequency = SpiChannel.MinFrequency;
            return pin.Read();
        }
    }

}
