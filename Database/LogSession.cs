using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewtal.Database
{
    public class LogSession
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public string Name { get; set; }
        public List<LogRecord> LogRecords { get; set; }

    }
}
