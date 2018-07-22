using HNS.BusinessSvcs;
using HNS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HNS.Entities.Qna;

namespace HNS.ViewModels
{
    public class QnaViewModel : IQnaViewModel
    {
        #region Local Variables

        private readonly IQnaService _qnaService;

        #endregion

        #region ctor

        public QnaViewModel()
        {
            QnaSearchCriteria = new QnaSearchCriteriaViewModel();
            Qnas = new PagedResult<Qna>();
        }

        public QnaViewModel(IQnaService qnaService)
        {
            //QnaPaging = new QnaPagingViewModel();
            QnaSearchCriteria = new QnaSearchCriteriaViewModel();
            Qnas = new PagedResult<Qna>();

            _qnaService = qnaService;
        }

        #endregion

        #region SubClasses

        public class QnaPagerViewModel
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public bool IsCurrentPage { get; set; }
        }

        public class QnaSearchCriteriaViewModel
        {
            public SearchType SearchType { get; set; }
            public string SearchString { get; set; }
        }

        #endregion

        #region Properties

        //public QnaPagingViewModel QnaPaging { get; set; }
        public QnaSearchCriteriaViewModel QnaSearchCriteria { get; set; }
        public PagedResult<Qna> Qnas { get; set; }
        public int PageNumber { get; set; }

        #endregion

        #region Methods

        public async Task<QnaViewModel> GetAllQnas(int pageIndex)
        {
            Qnas = await _qnaService.SearchQnas(pageIndex, 15, SearchType.Title, "");
            return this;
        }

        public async Task<QnaViewModel> SearchQnas(QnaViewModel model)
        {
            if (model.QnaSearchCriteria != null && !string.IsNullOrWhiteSpace(model.QnaSearchCriteria.SearchString))
            {
                Qnas = await _qnaService.SearchQnas(model.PageNumber, 15, model.QnaSearchCriteria.SearchType, model.QnaSearchCriteria.SearchString);
            }
            else
            {
                Qnas = await _qnaService.SearchQnas(model.PageNumber, 15, 0, "");
            }

            return this;
        }

        #endregion
    }
}
