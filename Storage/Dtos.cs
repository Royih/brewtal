
using System;
using System.Collections.Generic;

namespace Brewtal2.Storage
{
    public class RuntimeDto
    {
        public int Id { get; set; }
        public int? CurrentSessionId { get; set; }
        public SessionDto CurrentSession { get; set; }
        public DateTime Startup { get; set; }
    }

    public class TemplogDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public DateTime TimeStamp { get; set; }
        public double ActualTemperature { get; set; }
    }

    public class SessionLightDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public double TargetTemp { get; set; }
    }

    public class SessionDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public double TargetTemp { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public TimeSpan? TimeToReachTarget { get; set; }
        public List<TemplogDto> Logs { get; } = new List<TemplogDto>();

        public List<SessionLightDto> AllSessions { get; set; }


    }
}