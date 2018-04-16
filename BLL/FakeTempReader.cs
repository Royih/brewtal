using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Brewtal.Dtos;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace Brewtal.BLL
{
    public class FakeTempReader : ITempReader
    {
        private readonly ILogger<FakeTempReader> _logger;

        public FakeTempReader(ILogger<FakeTempReader> logger)
        {
            _logger = logger;
        }

        private double ReadTemp(int pidId)
        {
            var value = new Random().NextDouble() * 10;
            _logger.LogTrace($"Read fake temperature: {value}");
            return value;
        }

        public TempReaderResultDto ReadTemp()
        {
            return new TempReaderResultDto
            {
                Temp1 = ReadTemp(0),
                Temp2 = ReadTemp(1)
            };
        }
    }

}
