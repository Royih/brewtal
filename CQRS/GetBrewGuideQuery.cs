using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using brewtal.Extensions;
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
            var brew = _db.BrewSteps.Include(x => x.Brew).OrderByDescending(x => x.Order).FirstOrDefault(x => x.BrewId == query.BrewId);

            return new BrewGuideDto
            {
                Setup = new BrewDto
                {
                    Id = brew.Brew.Id,
                    BatchNumber = brew.Brew.BatchNumber,
                    BeginMash = brew.Brew.BeginMash.SpecifyUtcTime(),
                    Initiated = brew.Brew.Initiated.SpecifyUtcTime(),
                    Name = brew.Brew.Name,
                    MashTemp = brew.Brew.MashTemp,
                    StrikeTemp = brew.Brew.StrikeTemp,
                    SpargeTemp = brew.Brew.SpargeTemp,
                    MashOutTemp = brew.Brew.MashOutTemp,
                    MashTimeInMinutes = brew.Brew.MashTimeInMinutes,
                    BoilTimeInMinutes = brew.Brew.BoilTimeInMinutes,
                    BatchSize = brew.Brew.BatchSize,
                    MashWaterAmount = brew.Brew.MashWaterAmount,
                    SpargeWaterAmount = brew.Brew.SpargeWaterAmount
                },
                CurrentStep = new StepDto
                {
                    Id = brew.Id,
                    Order = brew.Order,
                    Name = brew.Name,
                    StartTime = brew.StartTime.SpecifyUtcTime(),
                    CompleteTime = brew.CompleteTime.SpecifyUtcTime(),
                    TargetMashTemp = brew.TargetMashTemp,
                    TargetSpargeTemp = brew.TargetSpargeTemp,
                    CompleteButtonText = brew.CompleteButtonText,
                    Instructions = brew.Instructions,
                    ShowTimer = brew.ShowTimer
                }
            };
        }
    }
}
