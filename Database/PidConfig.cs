using System;

namespace Brewtal.Database
{
    public class PidConfig
    {
        public int Id { get; set; }
        public int PidId { get; set; }
        public double PIDKp { get; set; }
        public double PIDKi { get; set; }
        public double PIDKd { get; set; }


    }
}
