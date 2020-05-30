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
    public class BrewsController : Controller
    {

        private readonly IMediator _mediator;

        public BrewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> List()
        {
            return Ok(await _mediator.Send(new ListBrewsQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> PostSaveUser(SaveBrewCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Route("getNew")]
        public async Task<IActionResult> GetNewUser()
        {
            return Ok(await _mediator.Send(new GetBrewQuery()));
        }

        [Route("get/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            return Ok(await _mediator.Send(new GetBrewQuery() { Id = id }));
        }

    }
}