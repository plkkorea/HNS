using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using HNS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    [Area("Admin")]
    public class PopupLayersController : Controller
    {
        private readonly IPopupLayerService _popupLayerService;

        public PopupLayersController(IPopupLayerService popupLayerService)
        {
            _popupLayerService = popupLayerService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _popupLayerService.GetPopupLayers();
            return View(list);
        }

        public async Task<IActionResult> PopupLayersEdit(int id)
        {
            var item = await _popupLayerService.GetPopupLayerById(id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> PopupLayersEdit(PopupLayer popupLayer)
        {
            if (popupLayer.Id == 0)
            {
                popupLayer.CreatedOn = DateTime.Now;
                popupLayer.CreatedBy = User.Identity.Name;
                await _popupLayerService.AddPopupLayer(popupLayer);
                return View(popupLayer);
            }

            await _popupLayerService.UpdatePopupLayer(popupLayer);
            return View(popupLayer);
        }

        public async Task<IActionResult> PopupLayersView(int id)
        {
            var item = await _popupLayerService.GetPopupLayerById(id);
            return View(item);
        }

        public async Task<IActionResult> PopupLayersDelete(int id)
        {
            await _popupLayerService.DeletePopupLayer(id);
            return RedirectToAction("Index");
        }
    }
}