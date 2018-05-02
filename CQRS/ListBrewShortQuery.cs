using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Brewtal.Extensions;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{

    public class ListBrewShortQuery : IRequest<IEnumerable<BrewShortDto>>
    {
        public string Id { get; set; }
    }

    public class ListBrewShortQueryHandler : RequestHandler<ListBrewShortQuery, IEnumerable<BrewShortDto>>
    {
        private readonly BrewtalContext _db;

        public ListBrewShortQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override IEnumerable<BrewShortDto> HandleCore(ListBrewShortQuery request)
        {
            var lastSteps = _db.BrewSteps.Include(x => x.Brew).GroupBy(x => x.BrewId).Select(x => x.OrderByDescending(y => y.Id).First()).ToList().OrderByDescending(x => x.Brew.BatchNumber);
            return lastSteps.Select(x => new BrewShortDto
            {
                Id = x.BrewId,
                Name = x.Brew.Name,
                BatchNumber = x.Brew.BatchNumber,
                BatchSize = x.Brew.BatchSize,
                BrewDate = x.Brew.BeginMash.SpecifyUtcTime(),
                CurrentStep = x.Name
            });
        }
    }
}
