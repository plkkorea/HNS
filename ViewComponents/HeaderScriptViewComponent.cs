using HNS.BusinessSvcs;
using HNS.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewComponents
{
    public class HeaderScriptViewComponent : ViewComponent
    {
        private readonly IHeaderScriptService _service;

        public HeaderScriptViewComponent(IHeaderScriptService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke()
        {
            var script = _service.GetHeaderScripts();
            if (script != null && script.Count > 0)
                return View("Default", script.FirstOrDefault(m => m.Active));
            else
                return View("Default");
        }
    }
}
