using System;
using System.Threading.Tasks;
using Brewtal.Dtos;

namespace Brewtal.BLL
{
    public class HeaterController
    {
        private TimeSpan _cycleTime = TimeSpan.FromSeconds(10);
        private double _percentage = 0;
        private readonly BrewIO _brewIO;
        private readonly Outputs _output;

        public HeaterController(BrewIO brewIO, Outputs output)
        {
            _brewIO = brewIO;
            _output = output;
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
                        SetPidValue(true);
                        CurrentStatus = true;
                        System.Threading.Thread.Sleep((int)timeOn);
                    }
                    if (timeOff > 0)
                    {
                        SetPidValue(false);
                        CurrentStatus = false;
                        System.Threading.Thread.Sleep((int)timeOff);
                    }
                });
            }
        }

        private void SetPidValue(bool value)
        {
            _brewIO.Set(_output,value);
        }

    }
}