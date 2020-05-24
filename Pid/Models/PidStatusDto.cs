using System;

namespace Brewtal2.Pid.Models
{

    public class PidStatusDto
    {
        public int PidId { get; set; }
        public string PidName { get; set; }
        public double TargetTemp { get; set; }
        public double CurrentTemp { get; set; }
        public bool Output { get; set; }
        public double OutputValue { get; set; }
        public double ErrorSum { get; set; }
    }

}