using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewModels
{
    public interface IProdCompareViewModel
    {
        Task<ProdCompareViewModel> SearchProducts(ProdCompareViewModel model);
    }
}
