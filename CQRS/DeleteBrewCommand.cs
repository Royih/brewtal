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

    public class DeleteBrewCommand : IRequest<CommandResultDto>
    {
        public BrewDto Brew { get; set; }
    }

    public class DeleteBrewCommandHandler : RequestHandler<DeleteBrewCommand, CommandResultDto>
    {
        private readonly BrewtalContext _db;

        public DeleteBrewCommandHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override CommandResultDto HandleCore(DeleteBrewCommand command)
        {
            _db.BrewSteps.RemoveRange(_db.BrewSteps.Where(x => x.BrewId == command.Brew.Id));
            var brew = _db.Brews.Single(x => x.Id == command.Brew.Id);
            _db.Brews.Remove(brew);
            _db.SaveChanges();
            return new CommandResultDto { Success = true };
        }
    }
}
