using System;
using System.Net;
using HNS.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HNS.TagHelpers
{
    //Reference: http://www.iaspnetcore.com/Blog/BlogPost/5a7e4ec89f90464d282c25b5/how-to-building-pager-tag-helper-in-aspnet-core-views

    [HtmlTargetElement("pager", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PagerTagHelper : TagHelper
    {
        public enum ButtonType
        {
            First,
            Previous,
            Page,
            Next,
            Last
        }

        private readonly HttpContext _httpContext;
        private readonly IUrlHelper _urlHelper;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public PagerTagHelper(IHttpContextAccessor accessor, IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _httpContext = accessor.HttpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        [HtmlAttributeName("pager-model")]
        public PagedResultBase Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Model == null)
            {
                return;
            }

            if (Model.PageCount == 0)
            {
                return;
            }

            var action = ViewContext.RouteData.Values["action"].ToString();
            //var urlTemplate = WebUtility.UrlDecode(_urlHelper.Action(action, new { @page = "{0}" }));
            var urlTemplate = WebUtility.UrlDecode(_urlHelper.Action(action));
            urlTemplate += "?page={0}";
            var request = _httpContext.Request;
            foreach (var key in request.Query.Keys)
            {
                if (key == "page")
                {
                    continue;
                }

                urlTemplate += "&" + key + "=" + request.Query[key];
            }

            var startIndex = Math.Max(Model.CurrentPage - 5, 1);
            var finishIndex = Math.Min(Model.CurrentPage + 5, Model.PageCount);

            output.TagName = "";
            //output.Content.AppendHtml("<ul class=\"pagination\">");
            if (Model.CurrentPage - 1 == 0)
            {
                AddPageLink(output, "javascript:;", "&laquo;", ButtonType.First);
                AddPageLink(output, "javascript:;", "Previous", ButtonType.Previous);
            }
            else
            {
                AddPageLink(output, string.Format(urlTemplate, 1), "&laquo;", ButtonType.First);
                AddPageLink(output, string.Format(urlTemplate, Model.CurrentPage - 1), "Previous", ButtonType.Previous);
            }

            for (var i = startIndex; i <= finishIndex; i++)
            {
                if (i == Model.CurrentPage)
                {
                    AddCurrentPageLink(output, i);
                }
                else
                {
                    AddPageLink(output, string.Format(urlTemplate, i), i.ToString(), ButtonType.Page);
                }
            }

            if (Model.CurrentPage + 1 > Model.PageCount)
            {
                AddPageLink(output, "javascript:;", "Next", ButtonType.Next);
                AddPageLink(output, "javascript:;", "&raquo;", ButtonType.Last);
            }
            else
            {
                AddPageLink(output, string.Format(urlTemplate, Model.CurrentPage + 1), "Next", ButtonType.Next);
                AddPageLink(output, string.Format(urlTemplate, Model.PageCount), "&raquo;", ButtonType.Last);
            }
            //output.Content.AppendHtml("</ul>");
        }

        private void AddPageLink(TagHelperOutput output, string url, string text, ButtonType buttonType)
        {
            //output.Content.AppendHtml("<li><a href=\"");
            //output.Content.AppendHtml(url);
            //output.Content.AppendHtml("\">");
            //output.Content.AppendHtml(text);
            //output.Content.AppendHtml("</a>");
            //output.Content.AppendHtml("</li>");

            output.Content.AppendHtml("<a href=\"");
            output.Content.AppendHtml(url);
            switch (buttonType)
            {
                case ButtonType.First:
                    output.Content.AppendHtml("\" class=\"arr fst\">");
                    break;
                case ButtonType.Previous:
                    output.Content.AppendHtml("\" class=\"arr prev\">");
                    break;
                case ButtonType.Page:
                    output.Content.AppendHtml("\" class=\"num\">");
                    break;
                case ButtonType.Next:
                    output.Content.AppendHtml("\" class=\"arr next\">");
                    break;
                case ButtonType.Last:
                    output.Content.AppendHtml("\" class=\"arr lst\">");
                    break;
                default:
                    break;
            }
            output.Content.AppendHtml(text);
            output.Content.AppendHtml("</a>");

        }

        private void AddCurrentPageLink(TagHelperOutput output, int page)
        {
            //output.Content.AppendHtml("<li class=\"active\">");
            //output.Content.AppendHtml("<span>");
            //output.Content.AppendHtml(page.ToString());
            //output.Content.AppendHtml("</span>");
            //output.Content.AppendHtml("</li>");

            output.Content.AppendHtml("<a href=\"");
            output.Content.AppendHtml("javascript:;\" class=\"num active\">");
            output.Content.AppendHtml(page.ToString());
            output.Content.AppendHtml("</a>");
        }
    }
}
