using System;

namespace Brewtal2.Models.Brews
{
    public enum Outputs
    {
        Pid1Output,
        Pid2Output,
        Output1,
        Output2
    }

    public class OutputDto
    {
        public Outputs Output { get; set; }
        internal int Pin { get; set; }
        public string Name { get; set; }
        public bool Value { get; set; }
        public bool Automatic { get; set; } /* Automatic is set from worker/internal logic. Manual is set from UI/user manually*/
    }

}