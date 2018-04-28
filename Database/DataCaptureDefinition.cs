using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewtal.Database
{
     public class DataCaptureDefinition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BrewStepTemplate")]
        public int BrewStepTemplateId { get; set; }
        public virtual BrewStepTemplate BrewStepTemplate { get; set; }
        public string Label { get; set; }
        public string ValueType { get; set; }
        public bool Optional { get; set; }
        public string Units { get; set; }
    }

}
