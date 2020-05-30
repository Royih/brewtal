using Brewtal2.Brews;
using Brewtal2.Brews.Models;
using Brewtal2.Infrastructure.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Commands
{

    public class SaveBrewCommand : IRequest<Brew>
    {
        public Brew Brew { get; set; }

    }

    public class SaveBrewCommandHandler : RequestHandler<SaveBrewCommand, Brew>
    {
        private readonly IBrewRepository _repo;
        private readonly ICurrentUser _currentUser;

        public SaveBrewCommandHandler(IBrewRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        protected override Brew Handle(SaveBrewCommand command)
        {
            return _repo.SaveBrew(command.Brew);
        }
    }
}