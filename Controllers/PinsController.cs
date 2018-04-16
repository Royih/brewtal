using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brewtal.BLL;

namespace Brewtal.Controllers
{
    /// Relays from left to right: 
    /// 1: pin 4
    /// 2: pin 22
    /// 3: pin 6
    /// 4: pin 26

    [Route("api/[controller]")]
    public class PinsController : Controller
    {
        private readonly IGPIO _gpio;
        public PinsController(IGPIO gpio)
        {
            _gpio = gpio;
        }

        public class Set
        {
            public int PinId { get; set; }
            public bool Status { get; set; }
        }


        [HttpPost("set")]
        public IActionResult PostSet([FromBody]Set command)
        {
            return Ok(_gpio.Set(command.PinId, command.Status));
        }

        [HttpGet("get/{pinId}")]
        public IActionResult Get(int pinId)
        {
            return Ok(_gpio.Get(pinId));
        }

    }
}
