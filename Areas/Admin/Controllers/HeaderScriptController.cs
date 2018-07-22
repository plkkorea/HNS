using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using HNS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Controllers
{
    [Authorize(Roles = "Administrators")]
    [Area("Admin")]
    public class HeaderScriptController : Controller
    {
        private readonly IHeaderScriptService _headerScriptService;

        public HeaderScriptController(IHeaderScriptService headerScriptService)
        {
            _headerScriptService = headerScriptService;
        }

        public IActionResult Index()
        {
            var script = _headerScriptService.GetHeaderScripts();
            if (script != null && script.Count > 0)
                return View(script.FirstOrDefault());
            else
                return View();
        }

        [HttpPost]
        public IActionResult Index(HeaderScript headerScript)
        {
            var scripts = _headerScriptService.GetHeaderScripts();
            if (scripts != null && scripts.Count > 0)
            {
                var script = scripts.FirstOrDefault();
                script.Active = headerScript.Active;
                script.Script = headerScript.Script;
                _headerScriptService.UpdateHeaderScript(script);
            }
            else
            {
                _headerScriptService.AddHeaderScript(headerScript);
            }

            return View(headerScript);
        }
    }
}