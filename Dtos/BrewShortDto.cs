using System;

namespace Brewtal.Dtos
{
    public class BrewShortDto
    {
        public int Id { get; set; }
        public int BatchNumber { get; set; }
        public string Name { get; set; }
        public float BatchSize { get; set; }
        public DateTime BrewDate { get; set; }
        public string CurrentStep { get; set; }
    }
}
