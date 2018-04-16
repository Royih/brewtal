using System;

namespace Brewtal.Dtos
{

    public class PidSatusDto
    {
        public int PidId { get; set; }
        public string PidName { get; set; }
        public double TargetTemp { get; set; }
        public double CurrentTemp { get; set; }
        public bool Output { get; set; }
        public DateTime ComputedTime { get; set; }
    }

    public class PidStatusesDto
    {
        public PidSatusDto[] Pids { get; set; }

    }

}