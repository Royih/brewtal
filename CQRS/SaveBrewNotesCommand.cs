using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using AutoMapper;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Brewtal.CQRS
{

    public class SaveBrewNotesCommand : IRequest<BrewDto>
    {
        public int BrewId { get; set; }
        public string Notes { get; set; }
    }

    public class SaveBrewNotesCommandHandler : RequestHandler<SaveBrewNotesCommand, BrewDto>
    {
        private readonly IAggregateRootFactory _arFactory;

        public SaveBrewNotesCommandHandler(IAggregateRootFactory arFactory)
        {
            _arFactory = arFactory;
        }

        protected override BrewDto HandleCore(SaveBrewNotesCommand command)
        {
            var brew = _arFactory.GetBrewById(command.BrewId);
            var savedBrew = brew.SaveBrewNotes(command.Notes);
            return Mapper.Map<BrewDto>(savedBrew);
        }
    }
}
