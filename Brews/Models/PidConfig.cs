using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Brewtal2.Brews.Models
{
    public class PidConfig
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public int PidId { get; set; }
        public double PIDKp { get; set; }
        public double PIDKi { get; set; }
        public double PIDKd { get; set; }


    }
}
