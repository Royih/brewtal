using System;

namespace Brewtal2.Pid.Models
{

    public class HardwareStatusDto
    {
        public PidStatusDto Pid { get; set; }
        public PidConfig PidConfig { get; set; }
        public DateTime ComputedTime { get; set; }
        public OutputDto[] ManualOutputs { get; set; }
    }

}