using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HNS.Areas.Admin.Models;
using HNS.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //string loggedIn = HttpContext.Session.GetString("login");
            //if (string.IsNullOrWhiteSpace(loggedIn))
            //{
            //    return RedirectToAction("Login");
            //}

            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if(result.Succeeded)
                {
                    TempData["SuccessMsg"] = "성공적으로 비밀번호가 변경되었습니다.";
                }
                else
                {
                    TempData["ErrorMsg"] = string.Join(" | ", result.Errors.Select(m=>m.Description));
                    ModelState.AddModelError("Error", string.Join(" | ", result.Errors.Select(m => m.Description)));
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            //string loggedIn = HttpContext.Session.GetString("login");
            //if(!string.IsNullOrWhiteSpace(loggedIn))
            //{
            //    return RedirectToAction("Index");
            //}
            return View();
        }

        [HttpPost]
        public IActionResult Login(string password)
        {
            //if(!string.IsNullOrWhiteSpace(password))
            //{
            //    if(password=="123456")
            //    {
            //        HttpContext.Session.SetString("login",":O");
            //        return RedirectToAction("Index");
            //    }
            //}

            return View();
        }
    }

}