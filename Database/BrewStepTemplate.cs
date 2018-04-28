using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewtal.Database
{
    public class BrewStepTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }
        public string Target1TempFrom { get; set; }
        public string Target2TempFrom { get; set; }
        public string CompleteTimeAdd { get; set; }
        public bool ShowTimer { get; set; }
    }

}
