using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using HNS.Entities;

namespace HNS.ViewModels
{
    public class VisitorInfoViewModel
    {
        private readonly IVisitorsInfoService _visitorInfoService;

        public VisitorInfoViewModel(IVisitorsInfoService visitorInfoService)
        {
            _visitorInfoService = visitorInfoService;
        }

        public VisitorsInfo visitorsInfo { get; set; }

    }
}
