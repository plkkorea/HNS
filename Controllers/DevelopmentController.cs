using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Controllers
{
    public class DevelopmentController : Controller
    {
        public IActionResult UiDesign()
        {
            return View();
        }

        public IActionResult Promotion()
        {
            return View();
        }

        public IActionResult Guide()
        {
            return View();
        }
    }
}