using System;

namespace Brewtal.Dtos
{

    public class HardwareStatusDto
    {
        public PidSatusDto[] Pids { get; set; }
        public DateTime ComputedTime { get; set; }
        public string LoggingToName { get; set; }
        public OutputDto[] ManualOutputs { get; set; }

    }

}