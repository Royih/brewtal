using System;
using System.Threading.Tasks;
using Brewtal2.Infrastructure.Commands;
using Brewtal2.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brewtal2.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetCurrentUserQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> PostSaveUser(SaveUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [Route("list")]
        public async Task<IActionResult> GetUsersList()
        {
            return Ok(await _mediator.Send(new GetUsersListQuery()));
        }

        [Route("get/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            return Ok(await _mediator.Send(new GetUserQuery() { Id = id }));
        }

        [Route("listRoles/{id}")]
        public async Task<IActionResult> GetUserRoles(Guid id)
        {
            return Ok(await _mediator.Send(new ListUserRolesQuery { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("listRoles")]
        public async Task<IActionResult> ListRoles()
        {
            return Ok(await _mediator.Send(new ListRolesQuery()));
        }

        [Authorize(Roles = "Admin")]
        [Route("resetPassword")]
        public async Task<IActionResult> PostResetPassword(ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(1);
        }

        [Authorize(Roles = "Admin")]
        [Route("create")]
        public async Task<IActionResult> PostCreateUser(CreateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [Route("delete")]
        public async Task<IActionResult> PostDeleteUser(DeleteUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Route("updateownprofile")]
        public async Task<IActionResult> PostUpdateOwnProfile(UpdateOwnProfileCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Route("updateownpassword")]
        public async Task<IActionResult> PostChangeOwnPassword(UpdateOwnPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}