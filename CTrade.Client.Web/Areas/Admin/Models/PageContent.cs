using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTrade.Client.Core;

namespace CTrade.Client.Web.Areas.Admin.Models
{
    public class PageContentCreateViewModel
    {
        public string ReriectUrl { get; set; }
        public string Error { get; set; }
    }
    public class PageContentEditViewModel
    {
        public string PageId { get; set; }
        public string SiteId { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }

        internal PageContent AsPageContent()
        {
            return new PageContent
            {
                Id = PageId,
                SiteId = SiteId, 
                Title = Title,
                Content = Content
            };
        }
    }

    public class PageContentViewModel
    {
        public PageContentViewModel() { }
        internal PageContentViewModel(PageContent pageContent)
        {
            pageContent.NotNull();

            Id = pageContent.Id;
            SiteId = pageContent.SiteId;
            Title = pageContent.Title;
            Content = pageContent.Content;
        }

        internal PageContentViewModel(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            Error = errorMessage;
        }

        public string Id { get; set; }
        public string SiteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Error { get; set; }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(Error); } }
        public bool IsEmpty { get { return string.IsNullOrWhiteSpace(Id); } }
        [AllowHtml]
        public string EditUrl { get; set; }
        public string PreviewUrl { get; set; }
    }
}