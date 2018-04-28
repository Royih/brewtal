using System;
using System.Linq;
using AutoMapper;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;

namespace Brewtal.CQRS
{

    public class GetBrewQuery : IRequest<BrewDto>
    {
        public int BrewId { get; set; }
    }

    public class GetBrewQueryHandler : RequestHandler<GetBrewQuery, BrewDto>
    {
        private readonly BrewtalContext _db;

        public GetBrewQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override BrewDto HandleCore(GetBrewQuery query)
        {
            if (query.BrewId > 0)
            {
                var t = Mapper.Map<BrewDto>(_db.Brews.Single(x => x.Id == query.BrewId));
                return t;
            }
            else
            {
                var maxBrew = _db.Brews.Select(x => x.BatchNumber).OrderByDescending(x => x).FirstOrDefault();
                return new BrewDto
                {
                    BatchNumber = maxBrew + 1,
                    BeginMash = DateTime.UtcNow,
                    MashTemp = 67.0f,
                    StrikeTemp = 73.6f,
                    SpargeTemp = 75.6f,
                    MashOutTemp = 78.0f,
                    MashTimeInMinutes = 60,
                    BoilTimeInMinutes = 60,
                    BatchSize = 40,
                    MashWaterAmount = 30,
                    SpargeWaterAmount = 20
                };
            }
        }
    }
}
