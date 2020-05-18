using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Brewtal2.DataAccess;
using Brewtal2.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Brewtal2.BLL.Users.Commands
{

    public class ResetPasswordCommand : IRequest<CommandResultDto>
    {
        public UserDto User { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeat { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, CommandResultDto>
    {
        private readonly IRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordCommandHandler(IRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<CommandResultDto> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            if (command.NewPassword != command.NewPasswordRepeat)
            {
                throw new SecurityException("The passwords does not match");
            }
            var user = _repo.GetUser(command.User.Id);

            var tokenTask = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, tokenTask, command.NewPassword);
            return new CommandResultDto() { Success = true };
        }
    }
}