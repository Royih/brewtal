using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using brewtal.Extensions;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;

namespace Brewtal.CQRS
{

    public class ListBrewGuideHistoryQuery : IRequest<IEnumerable<BrewHistoryDto>>
    {
        public int BrewId { get; set; }
    }

    public class ListBrewGuideHistoryQueryHandler : RequestHandler<ListBrewGuideHistoryQuery, IEnumerable<BrewHistoryDto>>
    {
        private readonly BrewtalContext _db;

        public ListBrewGuideHistoryQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override IEnumerable<BrewHistoryDto> HandleCore(ListBrewGuideHistoryQuery query)
        {
            var t = _db.BrewSteps.Where(x => x.BrewId == query.BrewId).OrderBy(x => x.Order).ToArray();
            var list = new List<BrewHistoryDto>();
            for (var i = 0; i < t.Length; i++)
            {
                var thisLog = t[i];
                BrewStep next = null;
                if (i + 1 < t.Length)
                    next = t[i + 1];

                list.Add(
                    new BrewHistoryDto
                    {
                        Name = thisLog.Name,
                        Started = thisLog.StartTime.SpecifyUtcTime(),
                        Completed = next?.StartTime.SpecifyUtcTime()
                    });
            }
            return list.AsEnumerable();
        }
    }
}
