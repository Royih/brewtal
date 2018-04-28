using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{

    public class GoToNextStepCommand : IRequest
    {
        public int BrewId { get; set; }
    }

    public class GoToNextStepCommandHandler : RequestHandler<GoToNextStepCommand>
    {
        private readonly IAggregateRootFactory _arFact;

        public GoToNextStepCommandHandler(IAggregateRootFactory arFact)
        {
            _arFact = arFact;
        }

        protected override void HandleCore(GoToNextStepCommand command)
        {
            var brew = _arFact.GetBrewById(command.BrewId);
            brew.GoToNextStep();
        }
    }
}
