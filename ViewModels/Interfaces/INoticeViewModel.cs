using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewModels
{
    public interface INoticeViewModel
    {
        Task<NoticeViewModel> GetAllNotices(int pageIndex);
        Task<NoticeViewModel> SearchNotices(NoticeViewModel model);
    }
}
