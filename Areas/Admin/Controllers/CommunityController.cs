using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using HNS.BusinessSvcs;
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
    public class CommunityController : Controller
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly INoticeViewModel _noticeViewModel;
        private readonly IFaqViewModel _faqViewModel;
        private readonly IQnaViewModel _qnaViewModel;
        private readonly INoticeService _noticeService;
        private readonly IFaqService _faqService;
        private readonly IQnaService _qnaService;
        private IHttpContextAccessor _accessor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CommunityController(INoticeViewModel noticeViewModel,
            IFaqViewModel faqViewModel,
            INoticeService noticeService,
            IFaqService faqService,
            IHttpContextAccessor accessor,
            IQnaViewModel qnaViewModel,
            IQnaService qnaService,
            IHostingEnvironment hostingEnvironment,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _noticeService = noticeService;
            _noticeViewModel = noticeViewModel;
            _faqViewModel = faqViewModel;
            _faqService = faqService;
            _accessor = accessor;
            _qnaViewModel = qnaViewModel;
            _qnaService = qnaService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        #region Methods

        #region Common

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmThenAction confirmThenAction)
        {
            if (confirmThenAction != null)
            {
                //if (string.IsNullOrWhiteSpace(confirmThenAction.Password))
                //    return BadRequest("Please enter password.");

                //TempData["password"] = confirmThenAction.Password ?? "";

                //string password = TempData["password"] == null ? "" : TempData["password"].ToString();
                var notice = await _noticeService.GetNoticeById(confirmThenAction.Id);
                if (notice != null)
                {
                    return Ok();//비밀번호가 없을경우 삭제
                }
            }
            return BadRequest("잘못된 요청입니다. 다시 시도해주세요.");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmQna(ConfirmThenAction confirmThenAction)
        {
            if (confirmThenAction != null)
            {
                if (string.IsNullOrWhiteSpace(confirmThenAction.Password))
                    return BadRequest("비밀번호를 입력해주세요");

                TempData["password"] = confirmThenAction.Password ?? "";

                string password = TempData["password"] == null ? "" : TempData["password"].ToString();
                var qna = await _qnaService.GetQnaById(confirmThenAction.Id);
                if (qna != null)
                    if (!qna.m_passwd.Equals(password))
                    {
                        return StatusCode(401, "비밀번호가 일치하지 않습니다.");
                    }
                    else
                        return Ok();
            }
            return BadRequest("잘못된 요청입니다. 다시 시도해주세요.");
        }

        #endregion

        #region Notice

        public async Task<IActionResult> Notice(int? page)
        {
            if (!page.HasValue)
                page = 1;
            else
                if (page.Value == 0)
                page = 1;

            var searchModel = HttpContext.Session.GetObjectFromJson<NoticeViewModel>("NoticeViewModel");
            if (searchModel != null && searchModel.NoticeSearchCriteria != null && !string.IsNullOrWhiteSpace(searchModel.NoticeSearchCriteria.SearchString))
            {
                searchModel.PageNumber = page.Value;
                searchModel = await _noticeViewModel.SearchNotices(searchModel);
                return View(searchModel);
            }
            else
            {
                var model = await _noticeViewModel.GetAllNotices(page.Value);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Notice(NoticeViewModel model)
        {
            if (model.NoticeSearchCriteria != null && !string.IsNullOrWhiteSpace(model.NoticeSearchCriteria.SearchString))
            {
                model.PageNumber = 1;
                HttpContext.Session.SetObjectAsJson("NoticeViewModel", model);
                model = await _noticeViewModel.SearchNotices(model);
            }
            else
            {
                HttpContext.Session.Remove("NoticeViewModel");
                model.Notices.CurrentPage = 1;
                model = await _noticeViewModel.GetAllNotices(model.Notices.CurrentPage);
            }

            return View(model);
        }

        public async Task<IActionResult> NoticeView(int id)
        {
            try
            {
                var notice = await _noticeService.GetNoticeById(id);
                return View(notice);
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT-FOUND")
                {
                    TempData["ErrorMsg"] = "자료가 존재하지 않습니다.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                    throw ex;
            }
        }

        public async Task<IActionResult> NoticeEdit(int id)
        {
            if (id > 0)
            {
                var notice = await _noticeService.GetNoticeById(id);
                return View(notice);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> NoticeDelete(int id)
        {
            await _noticeService.DeleteNotice(id);
            return RedirectToAction("Notice");
        }

        [HttpPost]
        public async Task<IActionResult> NoticeEdit(Notice notice)
        {
            if (notice != null)
            {
                //notice.m_name = User.Identity.Name;
                notice.m_name = "HNS";

                if (notice.Id > 0)
                {
                    try
                    {
                        notice.m_date = DateTime.Now;
                        await _noticeService.UpdateNotice(notice);
                        TempData["SuccessMsg"] = "성공적으로 저장되었습니다.";
                        return RedirectToAction("Notice");
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "PASSWORD-NOT-MATCHED")
                            TempData["ErrorMsg"] = "비밀번호가 일치하지 않습니다.";
                        else
                            TempData["ErrorMsg"] = ex.Message;
                    }
                }
                else
                {
                    notice.m_date = DateTime.Now;
                    notice.m_read = 0;
                    try
                    {
                        await _noticeService.AddNotice(notice);
                        TempData["SuccessMsg"] = "성공적으로 저장되었습니다.";
                        return RedirectToAction("Notice");
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "PASSWORD-NOT-MATCHED")
                            TempData["ErrorMsg"] = "비밀번호가 일치하지 않습니다.";
                        else
                            TempData["ErrorMsg"] = ex.Message;
                    }
                }
            }
            return View(notice);
        }

        #endregion

        #region FAQ

        public async Task<IActionResult> FAQ(int? page, int? id)
        {
            if (id.HasValue && id.Value > 0)
                return RedirectToAction("FAQView", new { id = id });

            if (!page.HasValue)
                page = 1;
            else
                if (page.Value == 0)
                page = 1;

            var searchModel = HttpContext.Session.GetObjectFromJson<FaqViewModel>("FaqViewModel");

            if (searchModel != null && searchModel.FaqSearchCriteria != null && !string.IsNullOrWhiteSpace(searchModel.FaqSearchCriteria.SearchString))
            {
                searchModel.PageNumber = 1;
                HttpContext.Session.SetObjectAsJson("FaqViewModel", searchModel);
                searchModel = await _faqViewModel.SearchFaqs(searchModel);
                return View(searchModel);
            }
            else
            {
                var model = await _faqViewModel.GetAllFaqs(page.Value);
                return View(model);
            }
        }

        public async Task<IActionResult> FAQViewerForIframe(int id)
        {
            if (id == 0)
                return View();
            else
            {
                var faq = await _faqService.GetFaqById(id);
                return View(faq);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FAQ(FaqViewModel model)
        {
            if (model.FaqSearchCriteria != null && !string.IsNullOrWhiteSpace(model.FaqSearchCriteria.SearchString))
            {
                model.PageNumber = 1;
                HttpContext.Session.SetObjectAsJson("FaqViewModel", model);
                model = await _faqViewModel.SearchFaqs(model);
            }
            else
            {
                HttpContext.Session.Remove("FaqViewModel");
                model.Faqs.CurrentPage = 1;
                model = await _faqViewModel.GetAllFaqs(model.Faqs.CurrentPage);
            }

            return View(model);
        }

        public async Task<IActionResult> FAQEdit(int id)
        {
            if (id == 0)
                return View();
            else
            {
                var faq = await _faqService.GetFaqById(id);
                return View(faq);
            }
        }

        public async Task<IActionResult> FAQView(int id)
        {
            if (id == 0)
                return View();
            else
            {
                var faq = await _faqService.GetFaqById(id);
                return View(faq);
            }
        }

        public async Task<IActionResult> FAQDelete(int id)
        {
            await _faqService.DeleteFaq(id);
            return RedirectToAction("FAQ");
        }

        [HttpPost]
        public async Task<IActionResult> FAQEdit(Faq faq)
        {
            if (faq.Id == 0)
            {
                faq.m_name = User.Identity.Name;
                faq.m_date = DateTime.Now;
                faq.m_read = 0;
                faq.m_ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                await _faqService.AddFaq(faq);
                TempData["SuccessMsg"] = "저장되었습니다.";
                return RedirectToAction("FAQ");
            }
            else
            {
                try
                {
                    faq.m_date = DateTime.Now;
                    await _faqService.UpdateFaq(faq);
                    TempData["SuccessMsg"] = "저장되었습니다.";
                    return RedirectToAction("FAQ");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "PASSWORD-NOT-MATCHED")
                        TempData["ErrorMsg"] = "비밀번호가 일치하지 않습니다.";
                    else
                        TempData["ErrorMsg"] = ex.Message;
                }
            }
            return View(faq);
        }

        public async Task<IActionResult> FaqViewed(int id)
        {
            var faq = await _faqService.GetFaqById(id);
            faq.m_read++;
            await _faqService.UpdateFaq(faq);
            return Json(faq.m_read);
        }

        #endregion

        #region QnA
        public async Task<IActionResult> Qna(int? page)
        {
            if (!page.HasValue)
                page = 1;
            else
                if (page.Value == 0)
                page = 1;

            var searchModel = HttpContext.Session.GetObjectFromJson<QnaViewModel>("QnaViewModel");
            if (searchModel != null && searchModel.QnaSearchCriteria != null && !string.IsNullOrWhiteSpace(searchModel.QnaSearchCriteria.SearchString))
            {
                searchModel.PageNumber = page.Value;
                searchModel = await _qnaViewModel.SearchQnas(searchModel);
                return View(searchModel);
            }
            else
            {
                var model = await _qnaViewModel.GetAllQnas(page.Value);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Qna(QnaViewModel model)
        {
            if (model.QnaSearchCriteria != null && !string.IsNullOrWhiteSpace(model.QnaSearchCriteria.SearchString))
            {
                model.PageNumber = 1;
                HttpContext.Session.SetObjectAsJson("QnaViewModel", model);
                model = await _qnaViewModel.SearchQnas(model);
            }
            else
            {
                HttpContext.Session.Remove("QnaViewModel");
                model.Qnas.CurrentPage = 1;
                model = await _qnaViewModel.GetAllQnas(model.Qnas.CurrentPage);
            }

            return View(model);
        }

        public async Task<IActionResult> QnaView(int id)
        {
            try
            {
                var notice = await _qnaService.GetQnaById(id);
                return View(notice);
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT-FOUND")
                {
                    TempData["ErrorMsg"] = "Record does not exists.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                    throw ex;
            }
        }

        public async Task<IActionResult> QnaEdit(int id, bool answer = false)
        {
            if (id > 0)
            {
                if (answer)
                {
                    var que = await _qnaService.GetQnaById(id);
                    var allAns = await _qnaService.GetQnaAnswersByQueId(id);
                    string title = "";
                    for (int i = 1; i <= allAns.Count; i++)
                    {
                        title += "[답변] ";
                    }
                    title += que.m_title;

                    ModelState.Clear();

                    Qna a = new Qna() { Id = 0, m_ref = id, m_title = title, m_name = User.Identity.Name };
                    return View(a);
                }
                else
                {
                    var qna = await _qnaService.GetQnaById(id);
                    return View(qna);
                }
            }
            else
            {
                return View(new Qna() { m_name=User.Identity.Name });
            }
        }

        public IActionResult QnaAnswer(int id)
        {
            return RedirectToAction("QnaEdit", new { id = id, answer = true });
        }

        public async Task<IActionResult> QnaDelete(int id)
        {
            await _qnaService.DeleteQna(id);
            return RedirectToAction("Qna");
        }

        [HttpPost]
        public async Task<IActionResult> QnaEdit(Qna qna)
        {
            if (qna != null)
            {
                qna.m_name = User.Identity.Name;

                var file = await UploadFile(HttpContext);
                if (file != null)
                {
                    qna.m_filesize = file.FileSizeInKB;
                    qna.m_file_guid = file.GuidName;
                    qna.m_file_name = file.OriginalFileName;
                }

                //if qna contains m_ref it means user is submitting answer.
                if (qna.m_ref.HasValue && qna.m_ref.Value > 0)
                {
                    if (qna.Id == qna.m_ref.Value)
                        qna.Id = 0;
                }

                if (qna.Id > 0)
                {
                    try
                    {
                        qna.m_date = DateTime.Now;
                        await _qnaService.UpdateQna(qna);
                        TempData["SuccessMsg"] = "성공적으로 저장되었습니다.";
                        return RedirectToAction("Qna");
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "PASSWORD-NOT-MATCHED")
                            TempData["ErrorMsg"] = "비밀번호가 일치하지 않습니다.";
                        else if (ex.Message == "PASSWORD-EMPTY")
                            TempData["ErrorMsg"] = "비밀번호를 입력하세요.";
                        else
                        {
                            TempData["ErrorMsg"] = ex.Message;
                            //ModelState.AddModelError(string.Empty, ex.Message);
                        }
                    }
                }
                else
                {
                    qna.m_date = DateTime.Now;
                    qna.m_read = 0;
                    try
                    {
                        await _qnaService.AddQna(qna);
                        TempData["SuccessMsg"] = "성공적으로 저장되었습니다.";
                        return RedirectToAction("Qna");
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMsg"] = ex.Message;
                    }
                }
            }
            return View(qna);
        }

        public async Task<IActionResult> DownloadQnAFile(int id)
        {
            var qna = await _qnaService.GetQnaById(id);
            string filename = qna.m_file_guid;
            if (filename == null)
                return Content("filename not present");

            var QnAUserFilePath = _configuration["UserFilePath:QnAFiles"];

            var rootPath = _hostingEnvironment.WebRootPath;
            var path = Path.Combine(
                           rootPath,
                           QnAUserFilePath, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, FileUploadHelper.GetContentType(path), qna.m_file_name);
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
                    var QnAUserFilePath = _configuration["UserFilePath:QnAFiles"];

                    if (!Directory.Exists(Path.Combine(rootPath, QnAUserFilePath)))
                        Directory.CreateDirectory(Path.Combine(rootPath, QnAUserFilePath));

                    var UploadFile = Path.Combine(rootPath, QnAUserFilePath, guidName);
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

        #endregion

        #endregion

    }

    public class UploadFileInfo
    {
        public string OriginalFileName { get; set; }
        public string GuidName { get; set; }
        public double FileSizeInKB { get; set; }
    }

    public class ConfirmThenAction
    {
        public enum ActionTypes
        {
            Edit,
            Delete
        }

        public string Password { get; set; }
        public int Id { get; set; }
        public ActionTypes ActionType { get; set; }
    }
}