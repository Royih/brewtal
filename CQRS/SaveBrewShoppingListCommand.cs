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

    public class SaveBrewShoppingListCommand : IRequest<CommandResultDto>
    {
        public int BrewId { get; set; }
        public string ShoppingList { get; set; }
    }

    public class SaveBrewShoppingListCommandHandler : RequestHandler<SaveBrewShoppingListCommand, CommandResultDto>
    {
        private readonly IAggregateRootFactory _arFactory;

        public SaveBrewShoppingListCommandHandler(IAggregateRootFactory arFactory)
        {
            _arFactory = arFactory;
        }

        protected override CommandResultDto HandleCore(SaveBrewShoppingListCommand command)
        {
            var brew = _arFactory.GetBrewById(command.BrewId);
            brew.SaveShoppingList(command.ShoppingList);
            return new CommandResultDto { Success = true };
        }
    }
}
