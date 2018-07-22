using HNS.BusinessSvcs;
using HNS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HNS.Entities.TechNote;

namespace HNS.ViewModels
{
    public class TechNoteViewModel : ITechNoteViewModel
    {
        #region Local Variables

        private readonly ITechNoteService _techNoteService;

        #endregion

        #region ctor

        public TechNoteViewModel()
        {
            TechNoteSearchCriteria = new TechNoteSearchCriteriaViewModel();
            TechNotes = new PagedResult<TechNote>();
        }

        public TechNoteViewModel(ITechNoteService techNoteService)
        {
            //TechNotePaging = new TechNotePagingViewModel();
            TechNoteSearchCriteria = new TechNoteSearchCriteriaViewModel();
            TechNotes = new PagedResult<TechNote>();

            _techNoteService = techNoteService;
        }

        #endregion

        #region SubClasses

        public class TechNotePagerViewModel
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public bool IsCurrentPage { get; set; }
        }

        public class TechNoteSearchCriteriaViewModel
        {
            public SearchType SearchType { get; set; }
            public string SearchString { get; set; }
        }

        #endregion

        #region Properties

        //public TechNotePagingViewModel TechNotePaging { get; set; }
        public TechNoteSearchCriteriaViewModel TechNoteSearchCriteria { get; set; }
        public PagedResult<TechNote> TechNotes { get; set; }
        public int PageNumber { get; set; }

        #endregion

        #region Methods

        public async Task<TechNoteViewModel> GetAllTechNotes(int pageIndex)
        {
            TechNotes = await _techNoteService.SearchTechNotes(pageIndex, 15, SearchType.Title, "");
            return this;
        }

        public async Task<TechNoteViewModel> SearchTechNotes(TechNoteViewModel model)
        {
            if (model.TechNoteSearchCriteria != null && !string.IsNullOrWhiteSpace(model.TechNoteSearchCriteria.SearchString))
            {
                TechNotes = await _techNoteService.SearchTechNotes(model.PageNumber, 15, model.TechNoteSearchCriteria.SearchType, model.TechNoteSearchCriteria.SearchString);
            }
            else
            {
                TechNotes = await _techNoteService.SearchTechNotes(model.PageNumber, 15, 0, "");
            }

            return this;
        }

        #endregion
    }
}
