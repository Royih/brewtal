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

    public class SaveBrewShoppingListCommand : IRequest<BrewDto>
    {
        public int BrewId { get; set; }
        public string ShoppingList { get; set; }
    }

    public class SaveBrewShoppingListCommandHandler : RequestHandler<SaveBrewShoppingListCommand, BrewDto>
    {
        private readonly IAggregateRootFactory _arFactory;

        public SaveBrewShoppingListCommandHandler(IAggregateRootFactory arFactory)
        {
            _arFactory = arFactory;
        }

        protected override BrewDto HandleCore(SaveBrewShoppingListCommand command)
        {
            var brew = _arFactory.GetBrewById(command.BrewId);
            var savedBrew = brew.SaveShoppingList(command.ShoppingList);
            return Mapper.Map<BrewDto>(savedBrew);
        }
    }
}
