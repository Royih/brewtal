using System.Threading;
using System.Threading.Tasks;
using Brewtal2.DataAccess;
using Brewtal2.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Brewtal2.BLL.Users.Commands
{

    public class CreateUserCommand : IRequest<CommandResultDto>
    {

        public UserDto User { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeat { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommandResultDto>
    {

        private readonly IRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUser _currentUser;

        public CreateUserCommandHandler(IRepository repo, UserManager<ApplicationUser> userManager, ICurrentUser currentUser)
        {
            _repo = repo;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<CommandResultDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            return await _repo.CreateUser(command.User, command.NewPassword);

        }
    }

}