using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brewtal.BLL;
using MediatR;
using Brewtal.CQRS;

namespace Brewtal.Controllers
{

    [Route("api/[controller]")]
    public class PidController : Controller
    {
        private readonly IMediator _meditor;

        public PidController(IMediator meditor)
        {
            _meditor = meditor;
        }

        /*[HttpPost("updateTarget")]
        public async Task PostSet([FromBody]UpdatePidTargetCommand command)
        {
            await _meditor.Send(command);
        }*/

        [HttpGet("{pidId:int}")]
        public async Task<IActionResult> GetPidConfig(int pidId)
        {
            return Ok(await _meditor.Send(new GetPidConfigQuery { PidId = pidId }));
        }

        [HttpPost("updatePidConfig")]
        public async Task UpdatePidConfig([FromBody]UpdatePidConfigCommand command)
        {
            await _meditor.Send(command);
        }

    }
}
