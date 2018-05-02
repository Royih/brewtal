using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Brewtal.CQRS
{

    public class SaveBrewNotesCommand : IRequest<CommandResultDto>
    {
        public int BrewId { get; set; }
        public string Notes { get; set; }
    }

    public class SaveBrewNotesCommandHandler : RequestHandler<SaveBrewNotesCommand, CommandResultDto>
    {
        private readonly IAggregateRootFactory _arFactory;

        public SaveBrewNotesCommandHandler(IAggregateRootFactory arFactory)
        {
            _arFactory = arFactory;
        }

        protected override CommandResultDto HandleCore(SaveBrewNotesCommand command)
        {
            var brew = _arFactory.GetBrewById(command.BrewId);
            brew.SaveBrewNotes(command.Notes);
            return new CommandResultDto { Success = true };
        }
    }
}
