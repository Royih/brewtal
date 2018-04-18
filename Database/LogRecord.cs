using System;

namespace Brewtal.Database
{
    public class LogRecord
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double TargetTemp1 { get; set; }
        public double ActualTemp1 { get; set; }
        public bool Output1 { get; set; }
        public double TargetTemp2 { get; set; }
        public double ActualTemp2 { get; set; }
        public bool Output2 { get; set; }

        public int SessionId { get; set; }
        public LogSession Session { get; set; }

    }
}
