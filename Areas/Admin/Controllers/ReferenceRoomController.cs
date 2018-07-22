﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using HNS.Controllers;
using HNS.Entities;
using HNS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    [Area("Admin")]
    public class ReferenceRoomController : Controller
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly ITechNoteViewModel _techNoteViewModel;
        private readonly ITechNoteService _techNoteService;
        private readonly ISmartXRelatedService _smartXRelatedService;

        private IHttpContextAccessor _accessor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReferenceRoomController(IHttpContextAccessor accessor,
            IHostingEnvironment hostingEnvironment,
            ITechNoteViewModel techNoteViewModel,
            ITechNoteService techNoteService,
            ISmartXRelatedService smartXRelatedService,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _techNoteService = techNoteService;
            _hostingEnvironment = hostingEnvironment;
            _accessor = accessor;
            _techNoteViewModel = techNoteViewModel;
            _smartXRelatedService = smartXRelatedService;
            _configuration = configuration;
        }

        #region Common

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmThenAction confirmThenAction)
        {
            if (confirmThenAction != null)
            {
                if (string.IsNullOrWhiteSpace(confirmThenAction.Password))
                    return BadRequest("비밀번호를 입력하세요.");

                TempData["password"] = confirmThenAction.Password ?? "";

                string password = TempData["password"] == null ? "" : TempData["password"].ToString();
                var notice = await _techNoteService.GetTechNoteById(confirmThenAction.Id);
                if (notice != null)
                {
                    return Ok();//비밀번호가 없을경우 삭제
                }
            }
            return BadRequest("잘못된 요청입니다. 다시 시도해주세요.");
        }

        #endregion


        public IActionResult ProductRelated()
        {
            return View();
        }

        #region SmartxRelated

        public async Task<IActionResult> SmartxRelated()
        {
            var model = await _smartXRelatedService.GetSmartXRelateds();
            return View(model);
        }

        public async Task<IActionResult> SmartxRelatedAdmin()
        {
            var model = await _smartXRelatedService.GetSmartXRelateds();
            return View(model);
        }

        public async Task<IActionResult> SmartxRelatedView(int? id)
        {
            if (id.HasValue)
            {
                var model = await _smartXRelatedService.GetSmartXRelatedById(id.Value);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> SmartxRelatedDelete(int id)
        {


                await _smartXRelatedService.DeleteSmartXRelated(id);
                return RedirectToAction("SmartxRelatedAdmin");


            //await _smartXRelatedService.DeleteSmartXRelated(id);
            //return RedirectToAction("SmartxRelated");


        }

        public async Task<IActionResult> SmartxRelatedEdit(int? id)
        {
            if (id.HasValue)
            {
                var model = await _smartXRelatedService.GetSmartXRelatedById(id.Value);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SmartxRelatedEdit(SmartXRelated model)
        {
            if (ModelState.IsValid)
            {
                model.Name = User.Identity.Name;

                try
                {
                    if (model.Id == 0)
                    {
                        model.DateTime = DateTime.Now;
                        await _smartXRelatedService.AddSmartXRelated(model);
                        TempData["SuccessMsg"] = "저장되었습니다.";
                        return RedirectToAction("SmartxRelatedAdmin");
                    }
                    else
                    {
                        model.DateTime = DateTime.Now;
                        await _smartXRelatedService.UpdateSmartXRelated(model);
                        TempData["SuccessMsg"] = "저장되었습니다.";
                        return RedirectToAction("SmartxRelatedAdmin");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = ex.Message;
                }
            }
            return View(model);
        }

        #endregion

        public IActionResult DrawingNApprover()
        {
            return View();
        }

        #region TechNotes

        public async Task<IActionResult> TechNote(int? page, int? id)
        {
            if (id.HasValue && id.Value > 0)
                return RedirectToAction("TechNoteView", new { id = id });

            if (!page.HasValue)
                page = 1;
            else
                if (page.Value == 0)
                page = 1;

            var searchModel = HttpContext.Session.GetObjectFromJson<TechNoteViewModel>("TechNoteViewModel");

            if (searchModel != null && searchModel.TechNoteSearchCriteria != null && !string.IsNullOrWhiteSpace(searchModel.TechNoteSearchCriteria.SearchString))
            {
                searchModel.PageNumber = 1;
                HttpContext.Session.SetObjectAsJson("TechNoteViewModel", searchModel);
                searchModel = await _techNoteViewModel.SearchTechNotes(searchModel);
                return View(searchModel);
            }
            else
            {
                var model = await _techNoteViewModel.GetAllTechNotes(page.Value);
                return View(model);
            }
        }

        public async Task<IActionResult> TechNoteViewerForIframe(int id)
        {
            if (id == 0)
                return View();
            else
            {
                var techNote = await _techNoteService.GetTechNoteById(id);
                return View(techNote);
            }
        }

        [HttpPost]
        public async Task<IActionResult> TechNote(TechNoteViewModel model)
        {
            if (model.TechNoteSearchCriteria != null && !string.IsNullOrWhiteSpace(model.TechNoteSearchCriteria.SearchString))
            {
                model.PageNumber = 1;
                HttpContext.Session.SetObjectAsJson("TechNoteViewModel", model);
                model = await _techNoteViewModel.SearchTechNotes(model);
            }
            else
            {
                HttpContext.Session.Remove("TechNoteViewModel");
                model.TechNotes.CurrentPage = 1;
                model = await _techNoteViewModel.GetAllTechNotes(model.TechNotes.CurrentPage);
            }

            return View(model);
        }

        public async Task<IActionResult> TechNoteEdit(int id)
        {
            if (id == 0)
                return View();
            else
            {
                var techNote = await _techNoteService.GetTechNoteById(id);
                return View(techNote);
            }
        }

        public async Task<IActionResult> TechNoteView(int id)
        {
            if (id == 0)
                return View();
            else
            {
                var techNote = await _techNoteService.GetTechNoteById(id);
                return View(techNote);
            }
        }

        public async Task<IActionResult> TechNoteDelete(int id)
        {
            await _techNoteService.DeleteTechNote(id);
            return RedirectToAction("TechNote");
        }

        [HttpPost]
        public async Task<IActionResult> TechNoteEdit(TechNote techNote)
        {
            if (techNote == null)
                return View(techNote);

            techNote.m_name = User.Identity.Name;

            var file = await UploadFile(HttpContext);
            if (file != null)
            {
                techNote.m_filesize = file.FileSizeInKB;
                techNote.m_file_guid = file.GuidName;
                techNote.m_file_name = file.OriginalFileName;
            }

            if (techNote.Id == 0)
            {
                techNote.m_name = User.Identity.Name;
                techNote.m_date = DateTime.Now;
                techNote.m_read = 0;
                await _techNoteService.AddTechNote(techNote);
                TempData["SuccessMsg"] = "저장되었습니다.";
                return RedirectToAction("TechNote");
            }
            else
            {
                try
                {
                    techNote.m_date = DateTime.Now;
                    techNote.m_date_modified = DateTime.Now;
                    await _techNoteService.UpdateTechNote(techNote);
                    TempData["SuccessMsg"] = "저장되었습니다.";
                    return RedirectToAction("TechNote");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "PASSWORD-NOT-MATCHED")
                        TempData["ErrorMsg"] = "비밀번호가 일치하지 않습니다.";
                    else
                        TempData["ErrorMsg"] = ex.Message;
                }
            }
            return View(techNote);
        }

        public async Task<IActionResult> DownloadTechNoteFile(int id)
        {
            var techNote = await _techNoteService.GetTechNoteById(id);
            string filename = techNote.m_file_guid;
            if (filename == null)
                return Content("filename not present");

            var TechNotesUserFilePath = _configuration["UserFilePath:TechNotesFiles"];

            var rootPath = _hostingEnvironment.WebRootPath;
            var path = Path.Combine(
                           rootPath,
                           TechNotesUserFilePath, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, FileUploadHelper.GetContentType(path), techNote.m_file_name);
        }

        public async Task<UploadFileInfo> UploadFile(HttpContext httpContext)
        {
            UploadFileInfo uploadFileInfo = null;
            if (httpContext.Request.Form.Files.Count > 0)
            {
                var file = httpContext.Request.Form.Files.FirstOrDefault();
                if (file.Length > 0)
                {
                    uploadFileInfo = new UploadFileInfo();
                    var rootPath = _hostingEnvironment.WebRootPath;
                    //No more we are using guid name as client has given his own folder mappings. And guid will make him difficult to search files.
                    var guidName = file.FileName;//Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var TechNotesUserFilePath = _configuration["UserFilePath:TechNotesFiles"];

                    if (!Directory.Exists(Path.Combine(rootPath, TechNotesUserFilePath)))
                        Directory.CreateDirectory(Path.Combine(rootPath, TechNotesUserFilePath));

                    var UploadFile = Path.Combine(rootPath, TechNotesUserFilePath, guidName);
                    uploadFileInfo.OriginalFileName = file.FileName;
                    uploadFileInfo.GuidName = guidName;
                    uploadFileInfo.FileSizeInKB = file.Length > 1024 ? file.Length / 1024 : 1;

                    using (var stream = new FileStream(UploadFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            return uploadFileInfo;
        }


        public async Task<IActionResult> TechNoteViewed(int id)
        {
            var techNote = await _techNoteService.GetTechNoteById(id);
            techNote.m_read++;
            await _techNoteService.UpdateTechNote(techNote, true);
            return Json(techNote.m_read);
        }


        #endregion

        public IActionResult ConnectedDevice()
        {
            return View();
        }

    }
}