using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brewtal.CQRS;
using Brewtal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Brewtal.Controllers
{
    [Route("api/[controller]")]
    public class BrewGuideController : Controller
    {
        private readonly IMediator _mediator;

        public BrewGuideController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfBrews()
        {
            return Ok(await _mediator.Send(new ListBrewShortQuery()));
        }

        [Route("setup/{brewId:int}")]
        public async Task<IActionResult> GetBrewData(int brewId)
        {
            return Ok(await _mediator.Send(new GetBrewQuery { BrewId = brewId }));
        }

        [Route("saveSetup")]
        public async Task<IActionResult> PostSave([FromBody]BrewDto brew)
        {
            return Ok(await _mediator.Send(new SaveBrewCommand { Brew = brew }));
        }

        [Route("delete")]
        public async Task<IActionResult> PostDelete([FromBody]BrewDto brew)
        {
            return Ok(await _mediator.Send(new DeleteBrewCommand { Brew = brew }));
        }

        [HttpGet]
        [Route("{brewId:int}")]
        public async Task<IActionResult> Get(int brewId)
        {
            return Ok(await _mediator.Send(new GetBrewGuideQuery { BrewId = brewId }));
        }

        [HttpGet]
        [Route("getBrewHistory/{brewId:int}")]
        public async Task<IActionResult> GetBrewHistory(int brewId)
        {
            return Ok(await _mediator.Send(new ListBrewGuideHistoryQuery { BrewId = brewId }));
        }

        [HttpPost]
        [Route("goToNextStep")]
        public async Task<IActionResult> GoToNextStep([FromBody]GoToNextStepCommand command)
        {
            await _mediator.Send(command);
            return Ok(true);
        }

        [HttpPost]
        [Route("goBackOneStep")]
        public async Task<IActionResult> GoToPreviousStep([FromBody]GoBackOneStepCommand command)
        {
            await _mediator.Send(command);
            return Ok(true);
        }

        [HttpPost("saveNotes")]
        public async Task<IActionResult> SaveNotes([FromBody]SaveBrewNotesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("saveShoppingList")]
        public async Task<IActionResult> SaveShoppingList([FromBody]SaveBrewShoppingListCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        [Route("getLatest")]
        public async Task<IActionResult> GetLatestBrew()
        {
            return Ok(await _mediator.Send(new GetLatestBrewIdQuery()));
        }

    }
}
