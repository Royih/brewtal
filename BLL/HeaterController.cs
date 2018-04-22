using System;
using System.Threading.Tasks;

namespace Brewtal.BLL
{
    public class HeaterController
    {
        private TimeSpan _cycleTime = TimeSpan.FromSeconds(10);
        private double _percentage = 0;
        private readonly IGPIO _gpio;
        private readonly int _outputPin;

        public HeaterController(IGPIO gpio, int outputPin)
        {
            _gpio = gpio;
            _outputPin = outputPin;
        }

        public void UpdateNextCyclePercentage(double percentage)
        {
            _percentage = percentage;
        }


        public bool CurrentStatus { get; private set; }

        public async void Start()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    var timeOn = (_cycleTime.TotalMilliseconds * _percentage / 100);
                    var timeOff = _cycleTime.TotalMilliseconds - timeOn;
                    if (timeOn > 0)
                    {
                        _gpio.Set(_outputPin, true);
                        CurrentStatus = true;
                        System.Threading.Thread.Sleep((int)timeOn);
                    }
                    if (timeOff > 0)
                    {
                        _gpio.Set(_outputPin, false);
                        CurrentStatus = false;
                        System.Threading.Thread.Sleep((int)timeOff);
                    }
                });
            }
        }

    }
}