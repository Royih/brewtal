using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Brewtal2.Models.Brews
{
    public class BrewStepTemplate
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }
        public string Target1TempFrom { get; set; }
        public string Target2TempFrom { get; set; }
        public string CompleteTimeAdd { get; set; }
        public bool ShowTimer { get; set; }
        public DataCaptureDefinition[] DataCaptureDefinitions { get; set; }
    }

    public class DataCaptureDefinition
    {
        public string Label { get; set; }
        public string ValueType { get; set; }
        public bool Optional { get; set; }
        public string Units { get; set; }
    }

}