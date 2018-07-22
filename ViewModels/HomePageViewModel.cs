using HNS.BusinessSvcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewModels
{
    public class HomePageViewModel
    {
        private readonly INoticeService _noticeService;

        public HomePageViewModel(INoticeService noticeService)
        {
            _noticeService = noticeService;
            Notices = _noticeService.GetNoticesTopN(6);
        }

        public List<Entities.Notice> Notices { get; set; }
    }
}
