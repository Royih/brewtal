using System;
using System.Collections.Generic;

namespace Brewtal.Dtos
{
    public class LogSessionDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public string Name { get; set; }
        public int LogPoints { get; set; }
        public string Duration
        {
            get
            {
                var ts = (Completed ?? DateTime.Now).Subtract(Created);
                return ts.ToString("hh\\:mm\\:ss");
            }
        }

    }
}
