using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewtal2.Controllers
{

    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SampleDataController : ControllerBase
    {
        public class WeatherForecastDto
        {
            public string DateFormatted { get; set; }

            public int TemperatureC { get; set; }

            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

            public string Summary { get; set; }
        }

        private static readonly string[] Summaries = new []
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        private readonly ILogger<SampleDataController> _logger;

        public SampleDataController(ILogger<SampleDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet("[Action]")]
        public IEnumerable<WeatherForecastDto> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
                {
                    DateFormatted = DateTime.Now.AddDays(index).ToString(),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpGet("[Action]")]
        public IEnumerable<dynamic> ListOptions()
        {
            yield return new { key = "1", value = "Option 1" };
            yield return new { key = "2", value = "Option 2" };
            yield return new { key = "3", value = "Option 3" };
            yield return new { key = "99", value = $"Option 4 {DateTime.Now.ToString()}" };
        }

        [HttpGet("[Action]")]
        public IActionResult Throw401()
        {
            return new UnauthorizedResult();
        }

        [HttpGet("[Action]")]
        public IActionResult Throw403()
        {
            return new ForbidResult();
        }

        [HttpGet("[Action]")]
        public IActionResult Throw500()
        {
            throw new Exception("This is a sample Exception!!");
        }
    }
}