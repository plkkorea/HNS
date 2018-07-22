using HNS.BusinessSvcs;
using HNS.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        private readonly IMenuItemService _menuItemService;

        public TopMenuViewComponent(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        //public IViewComponentResult Invoke()
        //{
        //    var controllerName = ViewContext.RouteData.Values["Controller"].ToString();
        //    var actionName = ViewContext.RouteData.Values["Action"].ToString();

        //    actionName = actionName.Replace("View", "").Replace("Edit", "");

        //    var menu = _menuItemService.GetMenu(controllerName, actionName);

        //    return View("Default", menu);
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var controllerName = ViewContext.RouteData.Values["Controller"].ToString();
            var actionName = ViewContext.RouteData.Values["Action"].ToString();

            actionName = actionName.Replace("View", "").Replace("Edit", "");

            var menu = await _menuItemService.GetMenuAsync(controllerName, actionName);

            return View("Default", menu);
        }
    }
}
