using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Brewtal2.Brews.Models
{
    public class LogSession
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public string Name { get; set; }
        public LogRecord[] LogRecords { get; set; }

    }

    public class LogRecord
    {
        public DateTime TimeStamp { get; set; }
        public double TargetTemp1 { get; set; }
        public double ActualTemp1 { get; set; }
        public bool Output1 { get; set; }
        public double TargetTemp2 { get; set; }
        public double ActualTemp2 { get; set; }
        public bool Output2 { get; set; }
    }
}