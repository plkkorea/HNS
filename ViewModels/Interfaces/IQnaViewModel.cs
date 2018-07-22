﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.ViewModels
{
    public interface IQnaViewModel
    {
        Task<QnaViewModel> GetAllQnas(int pageIndex);
        Task<QnaViewModel> SearchQnas(QnaViewModel model);
    }
}
