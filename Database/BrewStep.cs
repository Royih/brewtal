using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewtal.Database
{
    public class BrewStep
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Brew")]
        public int BrewId { get; set; }
        public virtual Brew Brew { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public float TargetMashTemp { get; set; }
        public float TargetSpargeTemp { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }
        public bool ShowTimer { get; set; }
    }

}
