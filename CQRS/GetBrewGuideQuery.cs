using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brewtal.Extensions;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{

    public class GetBrewGuideQuery : IRequest<BrewGuideDto>
    {
        public int BrewId { get; set; }
    }

    public class GetBrewGuideQueryHandler : RequestHandler<GetBrewGuideQuery, BrewGuideDto>
    {
        private readonly BrewtalContext _db;

        public GetBrewGuideQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override BrewGuideDto HandleCore(GetBrewGuideQuery query)
        {
            var brewStep = _db.BrewSteps.Include(x => x.Brew).OrderByDescending(x => x.Order).FirstOrDefault(x => x.BrewId == query.BrewId);
            var brew = Mapper.Map<BrewDto>(brewStep.Brew);

            return new BrewGuideDto
            {
                Setup = brew,
                CurrentStep = Mapper.Map<StepDto>(brewStep)
            };
        }
    }
}
