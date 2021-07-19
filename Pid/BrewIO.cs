using System.Linq;
using Brewtal2.Pid.Models;

namespace Brewtal2.Pid
{
    public class BrewIO
    {

        private readonly IGPIO _gpio;
        private const int _outputPinPid1 = 21;
        private const int _outputPinPid2 = 20;
        private const int _outputPinOutput1 = 26;
        private const int _outputPinOutput2 = 19;

        public readonly OutputDto[] SupportedOutputs;

        public void Set(Outputs output, bool value)
        {
            var myOutput = SupportedOutputs.Single(x => x.Output == output);
            _gpio.Set(myOutput.Pin, value);
            myOutput.Value = value;
        }

        public BrewIO(IGPIO gpio)
        {
            _gpio = gpio;
            SupportedOutputs = new []
            {
                new OutputDto
                {
                Output = Outputs.Pid1Output,
                Pin = _outputPinPid1,
                Name = "Pid 1 Output",
                Value = _gpio.Get(_outputPinPid1),
                Automatic = true
                },
                new OutputDto
                {
                Output = Outputs.Pid2Output,
                Pin = _outputPinPid2,
                Name = "Pid 2 Output",
                Value = _gpio.Get(_outputPinPid2),
                Automatic = true
                },
                new OutputDto
                {
                Output = Outputs.Output1,
                Pin = _outputPinOutput1,
                Name = "Mash pump",
                Value = _gpio.Get(_outputPinOutput1)
                },
                new OutputDto
                {
                Output = Outputs.Output2,
                Pin = _outputPinOutput2,
                Name = "Sparge pump",
                Value = _gpio.Get(_outputPinOutput2)
                }
            };
        }

    }
}