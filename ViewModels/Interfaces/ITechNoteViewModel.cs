using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewModels
{
    public interface ITechNoteViewModel
    {
        Task<TechNoteViewModel> GetAllTechNotes(int pageIndex);
        Task<TechNoteViewModel> SearchTechNotes(TechNoteViewModel model);
    }
}
