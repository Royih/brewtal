using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewtal.Database
{
    public class Brew
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BatchNumber { get; set; }
        public DateTime Initiated { get; set; }
        public DateTime BeginMash { get; set; }
        public string Name { get; set; }
        public float MashTemp { get; set; }
        public float StrikeTemp { get; set; }
        public float SpargeTemp { get; set; }
        public float MashOutTemp { get; set; }
        public int MashTimeInMinutes { get; set; }
        public int BoilTimeInMinutes { get; set; }
        public int BatchSize { get; set; }
        public float MashWaterAmount { get; set; }
        public float SpargeWaterAmount { get; set; }
    }
}
