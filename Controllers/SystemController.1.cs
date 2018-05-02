using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brewtal.BLL;
using Brewtal.Extensions;

namespace Brewtal.Controllers
{

    [Route("api/[controller]")]
    public class SystemController : Controller
    {

        public SystemController()
        {

        }

        [HttpPost("reboot")]
        public void Reboot()
        {
            "sudo reboot 0".Bash();
        }

        [HttpPost("shutdown")]
        public void Shutdown()
        {
            "sudo shutdown 0".Bash();
        }

    }
}
