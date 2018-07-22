using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using HNS.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Controllers
{
    public class HardwareController : Controller
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IProductService _productService;
        private readonly IProductCompareService _productCompareService;
        //private readonly IProduct_InfoService _product_InfoService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HardwareController(IHostingEnvironment hostingEnvironment, IProductService productService,
            Microsoft.Extensions.Configuration.IConfiguration configuration, IProductCompareService productCompareService)
        {
            _productService = productService;
            _productCompareService = productCompareService;
            //_product_InfoService = product_InfoService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Application()
        {
            return View();
        }

        public IActionResult Lineup()
        {
            return View();
        }

        public IActionResult Compare()
        {
            ProdCompareViewModel prodCompareViewModel = new ProdCompareViewModel(_productCompareService);

            return View(prodCompareViewModel);
        }

        [HttpPost]
        public IActionResult Compare(ProdCompareViewModel model)
        {
            ProdCompareViewModel prodCompareViewModel = new ProdCompareViewModel(_productCompareService);
            prodCompareViewModel = prodCompareViewModel.Search(model);
            return Json(prodCompareViewModel);
        }

        [HttpPost]
        public IActionResult CompareProducts(string prod1, string prod2)
        {
            var products = _productCompareService.GetProducts();
            var p1 = products.FirstOrDefault(m => m.Product_Name == prod1);
            var p2 = products.FirstOrDefault(m => m.Product_Name == prod2);

            var rootPath = _hostingEnvironment.WebRootPath + "\\";// + _configuration["UserFilePath:ProductImages"];

            if (p1 != null)
            {
                string folder = Path.Combine(rootPath, p1.ImagePath); //HNS.IOHelper.DirectorySearch(rootPath, prod1.Replace("IEC", ""));
                if (!string.IsNullOrWhiteSpace(folder))
                {
                    var files = Directory.GetFiles(folder, "*.jpg");
                    foreach (var file in files)
                    {
                        p1.Images.Add(Request.Headers["Origin"].ToString() + file.Replace(_hostingEnvironment.WebRootPath, "").Replace("\\", "/"));
                    }
                }
            }

            if (p2 != null)
            {
                string folder = Path.Combine(rootPath, p2.ImagePath);  //HNS.IOHelper.DirectorySearch(rootPath, prod2.Replace("IEC", ""));
                if (!string.IsNullOrWhiteSpace(folder))
                {
                    var files = Directory.GetFiles(folder, "*.jpg");
                    foreach (var file in files)
                    {
                        p2.Images.Add(Request.Headers["Origin"].ToString() + file.Replace(_hostingEnvironment.WebRootPath, "").Replace("\\", "/"));
                    }
                }
            }

            return Json(new { product1 = p1, product2 = p2 });
        }

        //[HttpPost]
        //public async Task<IActionResult> Compare(ProdCompareViewModel model,string[] LCD)
        //{
        //    if (model != null && model.ProdSearchCriteria != null)
        //    {
        //        if(model.ProdSearchCriteria.SearchType == null)
        //        {
        //            model.ProdSearchCriteria.SearchType = new List<Entities.Product_Info.SearchType>();
        //        }
        //        if(LCD.Count()>0)
        //        {
        //           foreach(var item in LCD)
        //            {
        //                model.ProdSearchCriteria.SearchType.Add(new Entities.Product_Info.SearchType() { LcdSize = item });
        //            }
        //        }

        //        model.PageNumber = 1;
        //        HttpContext.Session.SetObjectAsJson("ProdCompareViewModel", model);
        //        model = await _prodcompareViewModel.SearchProducts(model);
        //        return View(model);
        //    }
        //    else
        //    {
        //        model = HttpContext.Session.GetObjectFromJson<ProdCompareViewModel>("ProdCompareViewModel");
        //        return View(model);
        //    }
        //}

        public IActionResult Iec1000()
        {
            return View();
        }

        public IActionResult Iec266()
        {
            return View();
        }

        public IActionResult Iec667()
        {
            return View();
        }

        public IActionResult SmartIO()
        {
            return View();
        }

        public IActionResult Option()
        {
            return View();
        }

    }
}