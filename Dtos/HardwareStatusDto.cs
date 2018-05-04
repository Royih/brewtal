using System;

namespace Brewtal.Dtos
{

    public class HardwareStatusDto
    {
        public PidStatusDto[] Pids { get; set; }
        public DateTime ComputedTime { get; set; }
        public OutputDto[] ManualOutputs { get; set; }
    }

}