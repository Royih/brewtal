using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Brewtal2.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Brewtal2.Infrastructure.Commands
{

    public class UpdateOwnProfileCommand : IRequest<CommandResultDto>
    {
        public UserDto User { get; set; }
    }

    public class UpdateOwnProfileCommandHandler : IRequestHandler<UpdateOwnProfileCommand, CommandResultDto>
    {
        private readonly IAppRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateOwnProfileCommandHandler(IAppRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<CommandResultDto> Handle(UpdateOwnProfileCommand command, CancellationToken cancellationToken)
        {
            if (_repo.GetCurrentUser().Id.ToString() != command.User.Id.ToString())
            {
                throw new SecurityException("User is only allowed to update own profile!");
            }
            await _repo.SaveUser(command.User);

            return new CommandResultDto { Success = true };
        }
    }
}