using System;

namespace Brewtal2.Pid.Models
{

    public class HardwareStatusDto
    {
        public PidStatusDto[] Pids { get; set; }
        public DateTime ComputedTime { get; set; }
        public OutputDto[] ManualOutputs { get; set; }
    }

}