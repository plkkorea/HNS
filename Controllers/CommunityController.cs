using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using HNS.Entities;
using HNS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static HNS.Entities.Notice;

namespace HNS.Controllers
{
    public class CommunityController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly INoticeViewModel _noticeViewModel;
        private readonly IFaqViewModel _faqViewModel;
        private readonly IQnaViewModel _qnaViewModel;
        private readonly INoticeService _noticeService;
        private readonly IFaqService _faqService;
        private readonly IQnaService _qnaService;
        private IHttpContextAccessor _accessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDbRecordService _dbRecordService;

        public CommunityController(INoticeViewModel noticeViewModel,
            IFaqViewModel faqViewModel,
            INoticeService noticeService,
            IFaqService faqService,
            IHttpContextAccessor accessor,
            IQnaViewModel qnaViewModel,
            IQnaService qnaService,
            IHostingEnvironment hostingEnvironment,
            IDbRecordService dbRecordService,
            IConfiguration configuration)
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
            _dbRecordService = dbRecordService;
        }

        #region Methods

        #region Common

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmThenAction confirmThenAction)
        {
            if (confirmThenAction != null)
            {
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

        public async Task<IActionResult> Notice(int? page, int? searchtype, string query)
        {
            if (!page.HasValue)
                page = 1;
            else
                if (page.Value == 0)
                page = 1;

            NoticeViewModel searchModel = new NoticeViewModel();
            searchModel.PageNumber = page.Value;
            if (!string.IsNullOrWhiteSpace(query))
            {
                searchModel.NoticeSearchCriteria.SearchType = (SearchType)searchtype.Value;
                searchModel.NoticeSearchCriteria.SearchString = query;
            }

            searchModel = await _noticeViewModel.SearchNotices(searchModel);
            return View(searchModel);
        }

        [HttpPost]
        public async Task<IActionResult> Notice(NoticeViewModel model)
        {
            if (model.NoticeSearchCriteria != null && !string.IsNullOrWhiteSpace(model.NoticeSearchCriteria.SearchString))
            {
                model.PageNumber = 1;
                return RedirectToAction("Notice", new { page = 1, searchtype = (int)model.NoticeSearchCriteria.SearchType, query = model.NoticeSearchCriteria.SearchString });
            }
            else
            {
                model.Notices.CurrentPage = 1;
                return RedirectToAction("Notice", new { page = 1, searchtype = SearchType.Title, query = "" });
            }
        }

        public async Task<IActionResult> NoticeView(int id)
        {
            try
            {
                var notice = await _noticeService.GetNoticeById(id);

                var nextPrev = _dbRecordService.GetPrevNext("notice", id);
                ViewBag.NextPrev = nextPrev;

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

        [Authorize(Roles = "Administrators")]
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

        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> NoticeDelete(int id)
        {
            await _noticeService.DeleteNotice(id);
            return RedirectToAction("Notice");
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
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

        public async Task<IActionResult> NoticeViewerForIframe(int id)
        {
            if (id == 0)
                return View();
            else
            {
                var notice = await _noticeService.GetNoticeById(id);
                return View(notice);
            }
        }

        #endregion

        #region FAQ

        public async Task<IActionResult> FAQ(int? page, int? id, int? searchtype, string query)
        {
            if (id.HasValue && id.Value > 0)
                return RedirectToAction("FAQView", new { id = id });

            if (!page.HasValue)
                page = 1;
            else
                if (page.Value == 0)
                page = 1;


            FaqViewModel searchModel = new FaqViewModel();
            searchModel.PageNumber = page.Value;
            if (!string.IsNullOrWhiteSpace(query))
            {
                searchModel.FaqSearchCriteria.SearchType = (Faq.SearchType)searchtype.Value;
                searchModel.FaqSearchCriteria.SearchString = query;
            }

            searchModel = await _faqViewModel.SearchFaqs(searchModel);
            return View(searchModel);
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
                return RedirectToAction("FAQ", new { page = 1, searchtype = (int)model.FaqSearchCriteria.SearchType, query = model.FaqSearchCriteria.SearchString });
            }
            else
            {
                model.Faqs.CurrentPage = 1;
                return RedirectToAction("FAQ", new { page = 1, searchtype = SearchType.Title, query = "" });
            }
        }

        [Authorize(Roles = "Administrators")]
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

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> FAQEdit(Faq faq)
        {
            if (faq.Id == 0)
            {
                faq.m_name = "HNS";
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
        public async Task<IActionResult> Qna(int? page, int? searchtype, string query)
        {
            if (!page.HasValue)
                page = 1;
            else
                if (page.Value == 0)
                page = 1;

            QnaViewModel searchModel = new QnaViewModel();
            searchModel.PageNumber = page.Value;
            if (!string.IsNullOrWhiteSpace(query))
            {
                searchModel.QnaSearchCriteria.SearchType = (Qna.SearchType)searchtype.Value;
                searchModel.QnaSearchCriteria.SearchString = query;
            }

            searchModel = await _qnaViewModel.SearchQnas(searchModel);
            return View(searchModel);
        }

        [HttpPost]
        public async Task<IActionResult> Qna(QnaViewModel model)
        {
            if (model.QnaSearchCriteria != null && !string.IsNullOrWhiteSpace(model.QnaSearchCriteria.SearchString))
            {
                model.PageNumber = 1;
                return RedirectToAction("Qna", new { page = 1, searchtype = (int)model.QnaSearchCriteria.SearchType, query = model.QnaSearchCriteria.SearchString });
            }
            else
            {
                model.Qnas.CurrentPage = 1;
                return RedirectToAction("Qna", new { page = 1, searchtype = SearchType.Title, query = "" });
            }
        }

        public async Task<IActionResult> QnaView(int id)
        {
            try
            {
                var notice = await _qnaService.GetQnaById(id);
                var question = await _qnaService.GetQueByRefNo(notice.m_ref.Value);
                var prevAnswers = await _qnaService.GetPrevAnsById(id);
                if (!notice.IsQuestion && question != null)
                    notice.Question = question.m_content;

                string currentContent = notice.m_content == null ? "" : notice.m_content;

                if (!notice.IsQuestion && !(notice.m_content == null ? "" : notice.m_content).Contains("=============== 질 문 ==============="))
                {
                    notice.m_content = "=============== 질 문 ===============" + Environment.NewLine + notice.Question + Environment.NewLine + Environment.NewLine;

                    if (prevAnswers != null && prevAnswers.Count > 0)
                    {
                        foreach (var ans in prevAnswers)
                        {
                            notice.m_content += "=============== 답 변 ===============" + Environment.NewLine + ans.m_content + Environment.NewLine;
                        }
                    }

                    notice.m_content += "=============== 답 변 ===============" + Environment.NewLine + currentContent + Environment.NewLine;
                }
                if (notice.IsQuestion && !(notice.m_content == null ? "" : notice.m_content).Contains("=============== 질 문 ==============="))
                {
                    notice.m_content = "=============== 질 문 ===============" + Environment.NewLine + notice.m_content;
                }

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

        //[Authorize(Roles = "Administrators")]
        public async Task<IActionResult> QnaEdit(int id)
        {
            if (id > 0)
            {
                var qna = await _qnaService.GetQnaById(id);
                return View(qna);
            }
            else
            {
                return View();
            }
        }

        //[Authorize(Roles = "Administrators")]
        public async Task<IActionResult> QnaDelete(int id)
        {
            await _qnaService.DeleteQna(id);
            return RedirectToAction("Qna");
        }

        [HttpPost]
        //[Authorize(Roles = "Administrators")]
        public async Task<IActionResult> QnaEdit(Qna qna)
        {
            if (qna != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Administrators"))
                        qna.m_name = "HNS";
                    else
                        qna.m_name = User.Identity.Name;
                }


                var file = await UploadFile(HttpContext);
                if (file != null)
                {
                    qna.m_filesize = file.FileSizeInKB;
                    qna.m_file_guid = file.GuidName;
                    qna.m_file_name = file.OriginalFileName;
                }

                if (qna.Id > 0)
                {
                    try
                    {
                        qna.m_date = DateTime.Now;
                        await _qnaService.UpdateQna(qna);
                        TempData["SuccessMsg"] = "저장되었습니다.";
                        return RedirectToAction("Qna");
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
                    qna.m_date = DateTime.Now;
                    qna.m_read = 0;
                    try
                    {
                        await _qnaService.AddQna(qna);
                        TempData["SuccessMsg"] = "저장되었습니다.";
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