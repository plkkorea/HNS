using HNS.BusinessSvcs;
using HNS.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewComponents
{
    public class VisitorCountViewComponent : ViewComponent
    {
        private IHttpContextAccessor _accessor;
        private IVisitorsInfoService _visitorsInfoService;

        public VisitorCountViewComponent(IHttpContextAccessor accessor, IVisitorsInfoService visitorsInfoService)
        {
            _accessor = accessor;
            _visitorsInfoService = visitorsInfoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessionID = _accessor.HttpContext.Session.GetString("SessionID");

            if (string.IsNullOrEmpty(sessionID))
            {
                //Session Start
                _accessor.HttpContext.Session.SetString("SessionID", Guid.NewGuid().ToString());
                await _visitorsInfoService.AddVisitorCount();
            }

            return View("Default");
        }
    }
}
