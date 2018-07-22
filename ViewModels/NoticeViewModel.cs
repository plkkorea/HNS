using HNS.BusinessSvcs;
using HNS.Entities;
using HNS.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static HNS.Entities.Notice;

namespace HNS.ViewModels
{
    public class NoticeViewModel : INoticeViewModel
    {
        #region Local Variables

        private readonly INoticeService _noticeService;

        #endregion

        #region ctor

        public NoticeViewModel()
        {
            NoticeSearchCriteria = new NoticeSearchCriteriaViewModel();
            Notices = new PagedResult<Notice>();
            ImportantNotices = new List<Notice>();
        }

        public NoticeViewModel(INoticeService noticeService)
        {
            //NoticePaging = new NoticePagingViewModel();
            NoticeSearchCriteria = new NoticeSearchCriteriaViewModel();
            Notices = new PagedResult<Notice>();
            ImportantNotices = new List<Notice>();

            _noticeService = noticeService;
        }

        #endregion

        #region SubClasses

        //public class NoticePagingViewModel
        //{
        //    public NoticePagingViewModel()
        //    {
        //        Pages = new List<NoticePagerViewModel>();
        //    }
        //    public int CurrentPage { get; set; }
        //    public int TotalPage { get; set; }
        //    public int TotalRecords { get; set; }
        //    public List<NoticePagerViewModel> Pages { get; set; }
        //}

        public class NoticePagerViewModel
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public bool IsCurrentPage { get; set; }
        }

        public class NoticeSearchCriteriaViewModel
        {
            public SearchType SearchType { get; set; }
            public string SearchString { get; set; }
        }

        #endregion

        #region Properties

        //public NoticePagingViewModel NoticePaging { get; set; }
        public NoticeSearchCriteriaViewModel NoticeSearchCriteria { get; set; }
        public PagedResult<Notice> Notices { get; set; }
        public List<Notice> ImportantNotices { get; set; }
        public int PageNumber { get; set; }

        #endregion

        #region Methods

        public async Task<NoticeViewModel> GetAllNotices(int pageIndex)
        {
            Notices = await _noticeService.GetNotices(pageIndex, 15);
            ImportantNotices = await _noticeService.GetImportantNotices();
            return this;
        }

        public async Task<NoticeViewModel> SearchNotices(NoticeViewModel model)
        {
            if(model.NoticeSearchCriteria != null && !string.IsNullOrWhiteSpace(model.NoticeSearchCriteria.SearchString))
            {
                Notices = await _noticeService.SearchNotices(model.PageNumber, 15, model.NoticeSearchCriteria.SearchType, model.NoticeSearchCriteria.SearchString);
            }
            else
            {
                Notices = await _noticeService.SearchNotices(model.PageNumber, 15, 0, "");
            }

            ImportantNotices = await _noticeService.GetImportantNotices();

            return this;
        }

        #endregion
    }

}
