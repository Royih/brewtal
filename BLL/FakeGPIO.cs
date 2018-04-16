using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewtal.BLL
{
    public class FakeGPIO : IGPIO
    {
        private readonly ILogger<FakeGPIO> _logger;

        public FakeGPIO(ILogger<FakeGPIO> logger)
        {
            _logger = logger;
        }

        public bool Get(int pinId)
        {
            return false;
        }

        public bool Set(int pinId, bool status)
        {
            return status;
        }
    }

}
