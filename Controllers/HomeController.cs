using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HNS.Models;
using HNS.ViewModels;
using HNS.BusinessSvcs;

namespace HNS.Controllers
{
    public class HomeController : Controller
    {
        private readonly INoticeService _noticeService;
        private readonly IPopupLayerService _popupLayerService;
        //private readonly IVisitorsInfoService _visitorInfoService;

        public HomeController(INoticeService noticeService, IPopupLayerService popupLayerService)
        {
            _noticeService = noticeService;
            _popupLayerService = popupLayerService;
            //_visitorInfoService = visitorInfoService;
        }

        public async Task<IActionResult> Index()
        {
            HomePageViewModel homePageViewModel = new HomePageViewModel(_noticeService);

            var popup = await _popupLayerService.GetActivePopup();
            if(popup != null)
            {
                TempData["Popup"] = popup;
            }

            //VisitorInfoViewModel visitorInfoViewModel = new VisitorInfoViewModel(_visitorInfoService);
            ////Entities.VisitorsInfo objVisitorInfo = new Entities.VisitorsInfo();
            //visitorInfoViewModel.visitorsInfo.VisitorDate = DateTime.Now;
            //visitorInfoViewModel.visitorsInfo.NoOfVisitors = 1;
            //await _visitorInfoService.AddVisitorsInfo(visitorInfoViewModel.visitorsInfo);

            return View(homePageViewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
