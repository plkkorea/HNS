using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HNS.BusinessSvcs;
using HNS.Entities;

namespace HNS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    [Area("Admin")]
    public class VisitorsInfoController : Controller
    {
        private readonly IVisitorsInfoService _visitorInfoService;

        public VisitorsInfoController(IVisitorsInfoService visitorInfoService)
        {
            _visitorInfoService = visitorInfoService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _visitorInfoService.GetVisitorsInfos();
            return View(list);
        }
    }
}