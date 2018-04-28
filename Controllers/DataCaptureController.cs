using System.Collections.Generic;
using System.Threading.Tasks;
using Brewtal.CQRS;
using Brewtal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Brewmatic.Controllers
{
    [Route("api/[controller]")]
    public class DataCaptureController : Controller
    {

        private readonly IMediator _mediator;

        public DataCaptureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{brewStepId:int}")]
        public async Task<IActionResult> Get(int brewStepId)
        {
            return Ok(await _mediator.Send(new ListDataCaptureValuesQuery { BrewStepId = brewStepId }));  
        }
 
        [HttpPost]
        public async Task<IActionResult> Save([FromBody]DataCaptureValueDto[] values)
        {
            await _mediator.Send(new SaveDataCaptureCommand { Values = values });  
            return Ok(true);
        }

        [HttpGet]
        [Route("getDefinedValues/{brewId:int}")]
        public async Task<IActionResult> GetDefinedDataCaptureValues(int brewId)
        {
            return Ok(await _mediator.Send(new ListDataCaptureValueDefinitionsQuery { BrewId = brewId })); 
        }
    }
}
