using HNS.BusinessSvcs;
using HNS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HNS.Entities.Faq;

namespace HNS.ViewModels
{
    public class FaqViewModel : IFaqViewModel
    {
        #region Local Variables

        private readonly IFaqService _faqService;

        #endregion

        #region ctor

        public FaqViewModel()
        {
            FaqSearchCriteria = new FqaSearchCriteriaViewModel();
            Faqs = new PagedResult<Faq>();
        }

        public FaqViewModel(IFaqService faqService)
        {
            //FaqPaging = new FaqPagingViewModel();
            FaqSearchCriteria = new FqaSearchCriteriaViewModel();
            Faqs = new PagedResult<Faq>();

            _faqService = faqService;
        }

        #endregion

        #region SubClasses

        public class FaqPagerViewModel
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public bool IsCurrentPage { get; set; }
        }

        public class FqaSearchCriteriaViewModel
        {
            public SearchType SearchType { get; set; }
            public string SearchString { get; set; }
        }

        #endregion

        #region Properties

        //public FaqPagingViewModel FaqPaging { get; set; }
        public FqaSearchCriteriaViewModel FaqSearchCriteria { get; set; }
        public PagedResult<Faq> Faqs { get; set; }
        public int PageNumber { get; set; }

        #endregion

        #region Methods

        public async Task<FaqViewModel> GetAllFaqs(int pageIndex)
        {
            Faqs = await _faqService.GetFaqs(pageIndex, 100000);
            return this;
        }

        public async Task<FaqViewModel> SearchFaqs(FaqViewModel model)
        {
            if (model.FaqSearchCriteria != null && !string.IsNullOrWhiteSpace(model.FaqSearchCriteria.SearchString))
            {
                Faqs = await _faqService.SearchFaqs(model.PageNumber, 100000, model.FaqSearchCriteria.SearchType, model.FaqSearchCriteria.SearchString);
            }
            else
            {
                Faqs = await _faqService.SearchFaqs(model.PageNumber, 100000, 0, "");
            }

            return this;
        }

        #endregion
    }
}
