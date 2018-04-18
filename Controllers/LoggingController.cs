using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brewtal.BLL;
using Brewtal.Database;
using MediatR;

namespace Brewtal.Controllers
{

    [Route("api/[controller]")]
    public class LoggingController : Controller
    {
        private readonly IMediator _mediator;

        public LoggingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("start")]
        public async Task StartLogging([FromBody]StartLoggingCommand command)
        {
            await _mediator.Send(command);
        }

        [HttpPost("stop")]
        public async Task StopLogging([FromBody]StopLoggingCommand command)
        {
            await _mediator.Send(command);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListSessions()
        {
            return Ok(await _mediator.Send(new ListSessionsQuery()));
        }

        [HttpGet("get/{sessionId:int}")]
        public async Task<IActionResult> GetRecords(int sessionId)
        {
            return Ok(await _mediator.Send(new ListLogRecordsQuery { SessionId = sessionId }));
        }

        [HttpGet("get/{sessionId:int}/{numberOfSecondsInGroup:int}")]
        public async Task<IActionResult> GetRecordsBy10Seconds(int sessionId, int numberOfSecondsInGroup)
        {
            return Ok(await _mediator.Send(new ListLogRecordsQuery { SessionId = sessionId, NumberOfSecondsInGroup = numberOfSecondsInGroup }));
        }

    }
}
