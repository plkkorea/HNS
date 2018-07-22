using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Greeting()
        {
            return View();
        }

        public IActionResult Business()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}