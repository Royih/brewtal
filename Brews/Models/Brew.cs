using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Brewtal2.Brews.Models
{
    public class Brew
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public int BatchNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Initiated { get; set; }
        public DateTime BeginMash { get; set; }
        public string Name { get; set; }
        public decimal MashTemp { get; set; }
        public decimal StrikeTemp { get; set; }
        public decimal SpargeTemp { get; set; }
        public decimal MashOutTemp { get; set; }
        public int MashTimeInMinutes { get; set; }
        public int BoilTimeInMinutes { get; set; }
        public int BatchSize { get; set; }
        public decimal MashWaterAmount { get; set; }
        public decimal SpargeWaterAmount { get; set; }
        public string Notes { get; set; }
        public string ShoppingList { get; set; }
        public string OptimisticConcurrencyKey { get; set; }
        public BrewStep[] Steps { get; set; }

    }

    public class BrewStep
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public float TargetMashTemp { get; set; }
        public float TargetSpargeTemp { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }
        public bool ShowTimer { get; set; }

        public DataCaptureFloatValue[] FloatValues { get; set; }
        public DataCaptureIntValue[] IntValues { get; set; }
        public DataCaptureStringValue[] StringValues { get; set; }

    }

    public class DataCaptureFloatValue
    {
        public string Label { get; set; }
        public bool Optional { get; set; }
        public float? Value { get; set; }
        public string Units { get; set; }
    }

    public class DataCaptureIntValue
    {
        public string Label { get; set; }
        public bool Optional { get; set; }
        public int? Value { get; set; }
        public string Units { get; set; }
    }

    public class DataCaptureStringValue
    {
        public string Label { get; set; }
        public bool Optional { get; set; }
        public string Value { get; set; }
        public string Units { get; set; }
    }

}