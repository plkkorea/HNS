using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewModels
{
    public interface IFaqViewModel
    {
        Task<FaqViewModel> GetAllFaqs(int pageIndex);
        Task<FaqViewModel> SearchFaqs(FaqViewModel model);
    }
}
