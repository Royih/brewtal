using Brewtal2.Brews;
using Brewtal2.Brews.Models;
using Brewtal2.Infrastructure.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Commands
{

    public class DeleteBrewCommand : IRequest
    {
        public Brew Brew { get; set; }

    }

    public class DeleteBrewCommandHandler : RequestHandler<DeleteBrewCommand>
    {
        private readonly IBrewRepository _repo;
        private readonly ICurrentUser _currentUser;

        public DeleteBrewCommandHandler(IBrewRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        protected override void Handle(DeleteBrewCommand command)
        {
            _repo.DeleteBrew(command.Brew);
        }
    }
}