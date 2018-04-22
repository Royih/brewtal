using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace Brewtal.BLL
{
    public interface IGPIO
    {
        bool Set(int pinId, bool status);
        bool Get(int pinId);
    }
}
