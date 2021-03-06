using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewtal.Database
{
    public class DataCaptureStringValue
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BrewStep")]
        public int BrewStepId { get; set; }
        public virtual BrewStep BrewStep { get; set; }
        public string Label { get; set; }
        public bool Optional { get; set; }
        public string Value { get; set; }
        public string Units { get; set; }
    }

}
