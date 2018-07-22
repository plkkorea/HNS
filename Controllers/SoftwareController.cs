using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Controllers
{
    public class SoftwareController : Controller
    {
        public IActionResult SmartxFramework()
        {
            return View();
        }

        public IActionResult vbTool()
        {
            return View();
        }
    }
}