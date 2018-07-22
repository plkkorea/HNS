using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HNS.Controllers
{
    public class CustomerSupportController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CustomerSupportController(ISearchService searchService, IHostingEnvironment hostingEnvironment)
        {
            _searchService = searchService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult AsInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AsInfo(AsInfoViewModel asInfoViewModel, string Command)
        {
            ViewBag.Command = Command;

            if(Command=="Download")
            {
                var stream = GeneratePDF(asInfoViewModel);
                return File(stream.ToArray(), "application/pdf", "A/S 신청안내.pdf");
            }

            return View(asInfoViewModel);
        }

        public MemoryStream GeneratePDF(AsInfoViewModel asInfoViewModel)
        {
            System.IO.MemoryStream fs = new MemoryStream();
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            var rootPath = _hostingEnvironment.WebRootPath;
            //Korean Font
            var gulim = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");

            //Report Header
            iTextSharp.text.pdf.BaseFont bfntHead = iTextSharp.text.pdf.BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 16, 1, iTextSharp.text.BaseColor.GRAY);
            BaseFont fntHead = BaseFont.CreateFont(gulim, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font f = new Font(fntHead, 12, Font.NORMAL);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk("A/S 신청안내", f));
            document.Add(prgHeading);


            //Add a line seperation
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, iTextSharp.text.BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n", f));

            //Write the table
            PdfPTable table = new PdfPTable(2);
            ////Table header
            //BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //Font fntColumnHeader = new Font(btnColumnHeader, 10, 1, iTextSharp.text.BaseColor.WHITE);
            //for (int i = 0; i < dtblTable.Columns.Count; i++)
            //{
            //    PdfPCell cell = new PdfPCell();
            //    cell.BackgroundColor = Color.GRAY;
            //    cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
            //    table.AddCell(cell);
            //}
            //table Data

            table.AddCell(new Phrase("업체명", f));
            table.AddCell(new Phrase(asInfoViewModel.CompanyName, f));

            table.AddCell(new Phrase("담당자", f));
            table.AddCell(new Phrase(asInfoViewModel.Manager, f));

            table.AddCell(new Phrase("연락처", f));
            table.AddCell(new Phrase(asInfoViewModel.Contact, f));

            table.AddCell(new Phrase("FAX", f));
            table.AddCell(new Phrase(asInfoViewModel.Fax, f));

            table.AddCell(new Phrase("휴대전화", f));
            table.AddCell(new Phrase(asInfoViewModel.CellPhone, f));

            table.AddCell(new Phrase("이메일", f));
            table.AddCell(new Phrase(asInfoViewModel.Email, f));

            table.AddCell(new Phrase("제품명", f));
            table.AddCell(new Phrase(asInfoViewModel.ProductName, f));

            table.AddCell(new Phrase("Serial No.", f));
            table.AddCell(new Phrase(asInfoViewModel.SerialNumber, f));

            table.AddCell(new Phrase("증상", f));
            table.AddCell(new Phrase(asInfoViewModel.Symptom, f));

            table.AddCell(new Phrase("내용", f));
            table.AddCell(new Phrase(asInfoViewModel.Content, f));

            document.Add(table);
            document.Close();
            //writer.Close();
            //fs.Close();

            return fs;
        }

        public IActionResult RemoteTechSupport()
        {
            return View();
        }

        public IActionResult ProductPurchaseGuide()
        {
            return View();
        }

        #region Search

        public async Task<IActionResult> Search(string q, string utm)
        {
            TempData["q"] = q;

            if (string.IsNullOrWhiteSpace(q))
                return View();
            else
            {
                var result = await _searchService.Search(q);
                return View(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string q)
        {
            return RedirectToAction("Search", new { q = q });
            //TempData["q"] = q;

            //if (string.IsNullOrWhiteSpace(q))
            //    return View();
            //else
            //{
            //    var result = await _searchService.Search(q);
            //    return View(result);
            //}
        }

        public async Task<List<string>> SuggestKeywords(string term)
        {
            var result = await _searchService.GetAutoSuggestKeywords(term);
            return result;
        }

        #endregion
    }
}