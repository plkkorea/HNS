using HNS.BusinessSvcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewModels
{
    public class SmartXRelatedViewModel
    {
        private readonly ISmartXRelatedService _smartXRelatedService;

        public SmartXRelatedViewModel(ISmartXRelatedService smartXRelatedService)
        {
            _smartXRelatedService = smartXRelatedService;
        }


    }
}
