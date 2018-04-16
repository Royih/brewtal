using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brewtal.BLL;

namespace Brewtal.Controllers
{

    [Route("api/[controller]")]
    public class PidController : Controller
    {
        private PidWorker _pidWorker;

        public PidController(PidWorker pidWorker)
        {
            _pidWorker = pidWorker;
        }

        public class ChangeTargetCommand
        {
            public int PIDId { get; set; }
            public double NewTargetTemp { get; set; }
        }


        [HttpPost("update")]
        public void PostSet([FromBody]ChangeTargetCommand command)
        {
            _pidWorker.UpdateTargetTemp(command.PIDId, command.NewTargetTemp);
        }

    }
}
