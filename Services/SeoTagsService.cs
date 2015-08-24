using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment;
using Orchard.UI.Resources;
using Orchard.Utility.Extensions;
using Orchard.Localization;
using MainBit.Utility.Services;
using MainBit.SeoTags.Models;
using MainBit.SeoTags.Settings;


namespace MainBit.SeoTags.Services
{
    public class SeoTagsService : ISeoTagsService
    {

        private readonly IWorkContextAccessor _wca;
        private readonly IResourceManager _resourceManager;
        private readonly UrlHelper _urlHelper;
        private readonly ICurrentContentAccessor _currentContentAccessor;

        public SeoTagsService(
            IWorkContextAccessor wca,
            IResourceManager resourceManager,
            UrlHelper urlHelper,
            ICurrentContentAccessor currentContentAccessor)
        {
            _wca = wca;
            _resourceManager = resourceManager;
            _urlHelper = urlHelper;
            _currentContentAccessor = currentContentAccessor;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }


        public void RegisterMetaForList(
            int page,
            int pageSize,
            double totalItemCount,
            string PagerId)
        {
            
            #region prepare

            var workContext = _wca.GetContext();
            var currentPage = page < 1 ? 1 : page;

            if (pageSize < 1)
                pageSize = workContext.CurrentSite.PageSize;

            var totalPageCount = (int)Math.Ceiling(totalItemCount / pageSize);

            var routeData = workContext.HttpContext.Request.RequestContext.RouteData.Values;
            var queryString = workContext.HttpContext.Request.QueryString;

            var pageKey = String.IsNullOrEmpty(PagerId) ? "page" : PagerId;

            var contentItem = _currentContentAccessor.CurrentContentItem;
            SeoTagsPart seoTagsPart = null;
            SeoTagsPartSettings settings = null;
            if (contentItem != null)
            {
                seoTagsPart = contentItem.As<SeoTagsPart>();
                if (seoTagsPart != null)
                {
                    settings = seoTagsPart.TypePartDefinition.GetSeoTagsPartSettings();
                }
            }

            #endregion

            var canonical = _resourceManager.GetRegisteredLinks().FirstOrDefault(p => p.Rel == "canonical");
            if (canonical == null)
            {
                return;
            }
            var canonicalUrl = canonical.Href;

            if (queryString == null || queryString.Count == 0 || (queryString.Count == 1 && queryString[pageKey] != null))
            {
                // rel canonical
                if (currentPage > 1 && (settings == null || !settings.FirstPageCanonical))
                {
                    canonical.Href = AddKeyToUrl(canonicalUrl, pageKey, currentPage);
                }

                // rel prev
                if (currentPage > 1)
                {
                    string prevUrl;
                    if (currentPage == 2)
                    {
                        prevUrl = canonicalUrl;
                    }
                    else
                    {
                        prevUrl = AddKeyToUrl(canonicalUrl, pageKey, currentPage - 1);
                    }
                    _resourceManager.RegisterLink(new LinkEntry()
                    {
                        Href = prevUrl,
                        Rel = "prev"
                    });
                }

                // rel next
                if (totalPageCount > currentPage)
                {
                    _resourceManager.RegisterLink(new LinkEntry()
                    {
                        Href = AddKeyToUrl(canonicalUrl, pageKey, currentPage + 1),
                        Rel = "next"
                    });
                }
            }
            else
            {
                // rel canonical
                // rel canonical is already set by SeoTagsPartDriver to content item url (first page)
                
                // rel prev
                // rel prev doesn't need to be set because rel canonical points to first page (prev doesn't exist for a first page)

                // rel next
                // rel next tag is always added even a second page doesn't exist
                _resourceManager.RegisterLink(new LinkEntry()
                {
                    Href = AddKeyToUrl(canonicalUrl, pageKey, 2),
                    Rel = "next"
                });
            }

            // var appPath = workContext.HttpContext.Request.ToApplicationRootUrlString();
        }

        private string AddKeyToUrl(string url, string key, int value)
        {
            if (url.IndexOf('?') >= 0)
            {
                url += "&" + key + "=" + value;
            }
            else
            {
                url += "?" + key + "=" + value;
            }
            return url;
        }


        public string GetPageTitle(string title, int pageNumber)
        {
            return pageNumber <= 1
                ? title
                : string.Format(T("{0} - Page {1}").ToString(), title, pageNumber);
        }

        public string GetPageDescription(string description, int pageNumber)
        {
            return pageNumber <= 1
                ? description
                : string.Format(T("{0} Page {1}").ToString(), description, pageNumber);
        }
    }
}