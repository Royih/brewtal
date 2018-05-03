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

    public class SaveBrewCommand : IRequest<BrewDto>
    {
        public BrewDto Brew { get; set; }
    }

    public class SaveBrewCommandHandler : RequestHandler<SaveBrewCommand, BrewDto>
    {
        private readonly IAggregateRootFactory _arFactory;

        public SaveBrewCommandHandler(IAggregateRootFactory arFactory)
        {
            _arFactory = arFactory;
        }

        protected override BrewDto HandleCore(SaveBrewCommand command)
        {
            var brew = _arFactory.GetBrewById(command.Brew.Id);
            var savedBrew = brew.SaveBrew(command.Brew);
            return Mapper.Map<BrewDto>(savedBrew);
        }
    }
}
