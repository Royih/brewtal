using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Brewtal.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Brewtal.CQRS
{

    public class GoBackOneStepCommand : IRequest
    {
        public int BrewId { get; set; }
    }

    public class GoBackOneStepCommandHandler : RequestHandler<GoBackOneStepCommand>
    {
        private readonly IAggregateRootFactory _arFact;

        public GoBackOneStepCommandHandler(IAggregateRootFactory arFact)
        {
            _arFact = arFact;
        }

        protected override void HandleCore(GoBackOneStepCommand command)
        {
            var brew = _arFact.GetBrewById(command.BrewId);
            brew.GoBackOneStep();
        }
    }
}
