using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO;

namespace Brewtal2.BLL.Pid
{
    public interface IGPIO
    {
        bool Set(int pinId, bool status);
        bool Get(int pinId);
    }
}