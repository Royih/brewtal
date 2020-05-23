using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using Brewtal2.Infrastructure.Models;

namespace Brewtal2.Infrastructure.Commands
{

    public class DeleteUserCommand : IRequest<CommandResultDto>
    {
        public UserDto User { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, CommandResultDto>
    {
        private readonly IAppRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHandler(IAppRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<CommandResultDto> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var userNameAndEmail = command.User.UserName;
            var user = await _userManager.FindByNameAsync(command.User.UserName);
            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            var t = await _userManager.DeleteAsync(user);
            if (t.Succeeded)
            {
                return new CommandResultDto() { Success = true };
            }
            else
            {
                return new CommandResultDto { Success = false, ErrorMessages = t.Errors.Select(x => x.Description).ToArray() };
            }
        }
    }
}
