using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brewtal.Database;
using MediatR;

namespace Brewtal.CQRS
{

    public class GetLatestBrewIdQuery : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class GetLatestBrewIdQueryHandler : RequestHandler<GetLatestBrewIdQuery, int>
    {
        private readonly BrewtalContext _db;

        public GetLatestBrewIdQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override int HandleCore(GetLatestBrewIdQuery request)
        {
            var step = _db.BrewSteps.OrderByDescending(x => x.BrewId).ThenByDescending(x => x.Id).FirstOrDefault();
            if (step != null)
            {
                return step.BrewId;
            }
            return 0;
        }
    }
}
