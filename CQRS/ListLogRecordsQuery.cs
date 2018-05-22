
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{
    public class ListLogRecordsQuery : IRequest<IEnumerable<LogRecord>>
    {
        public int SessionId { get; set; }

        public int NumberOfSecondsInGroup { get; set; }
    }

    public class ListLogRecordsQueryHandler : RequestHandler<ListLogRecordsQuery, IEnumerable<LogRecord>>
    {
        private readonly BrewtalContext _db;

        public ListLogRecordsQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override IEnumerable<LogRecord> HandleCore(ListLogRecordsQuery query)
        {
            return _db.Records.Where(x => x.SessionId == query.SessionId)
            .GroupBy(x => new
            {
                TimeStamp = x.TimeStamp.Date.AddHours(x.TimeStamp.Hour).AddMinutes(x.TimeStamp.Minute).AddSeconds((x.TimeStamp.Second.RoundOff(query.NumberOfSecondsInGroup)))
            })
            .ToList() //need this to avoid a runtime exception
            .Select(x => new LogRecord
            {
                TimeStamp = x.Key.TimeStamp,
                ActualTemp1 = x.Average(y => y.ActualTemp1),
                TargetTemp1 = x.Min(y => y.TargetTemp1),
                Output1 = x.Min(y => y.Output1),
                ActualTemp2 = x.Average(y => y.ActualTemp2),
                TargetTemp2 = x.Min(y => y.TargetTemp2),
                Output2 = x.Min(y => y.Output2)
            });
        }
    }

    public static class ExtensionMethods
    {
        public static int RoundOff(this int i, int interval)
        {
            return ((int)Math.Round(i / (double)interval)) * interval;
        }
    }

}
