using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.Environment;
using Orchard.UI.Resources;
using Orchard.Utility.Extensions;
using Orchard.Localization;

namespace MainBit.SeoTags.Services
{
    public class SeoTagsService : ISeoTagsService {

        private readonly Work<WorkContext> _workContext;
        private readonly IResourceManager _resourceManager;
        private readonly UrlHelper _urlHelper;

        public SeoTagsService(
            Work<WorkContext> workContext,
            IResourceManager resourceManager,
            UrlHelper urlHelper)
        {
            _workContext = workContext;
            _resourceManager = resourceManager;
            _urlHelper = urlHelper;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }


        public void RegisterRelForList(
            int Page,
            int PageSize,
            double TotalItemCount,
            string PagerId)
        {
            var currentPage = Page < 1 ? 1 : Page;

            var pageSize = PageSize;
            if (pageSize < 1)
                pageSize = _workContext.Value.CurrentSite.PageSize;

            var totalPageCount = (int)Math.Ceiling(TotalItemCount / pageSize);

            var routeData = _workContext.Value.HttpContext.Request.RequestContext.RouteData.Values;
            var queryString = _workContext.Value.HttpContext.Request.QueryString;

            

            if (queryString != null)
            {
                foreach (var key in from string key in queryString.Keys where key != null && !routeData.ContainsKey(key) let value = queryString[key] select key)
                {
                    routeData[key] = queryString[key];
                }
            }

            // HACK: MVC 3 is adding a specific value in System.Web.Mvc.Html.ChildActionExtensions.ActionHelper
            // when a content item is set as home page, it is rendered by using Html.RenderAction, and the routeData is altered
            // This code removes this extra route value
            var removedKeys = routeData.Keys.Where(key => routeData[key] is DictionaryValueProvider<object>).ToList();
            foreach (var key in removedKeys)
            {
                routeData.Remove(key);
            }

            var pageKey = String.IsNullOrEmpty(PagerId) ? "page" : PagerId;

            var canonical = _resourceManager.GetRegisteredLinks().FirstOrDefault(p => p.Rel == "canonical");
            if (canonical == null) {
                return;
            }
            var canonicalUrl = canonical.Href;
            
            if (queryString == null || queryString.Count == 0 || (queryString.Count == 1 && queryString[pageKey] != null))
            {
                // rel canonical
                // можно закоментаровать, так как на avito.ru все укакзывает на первую страницу
                if (currentPage > 1) {
                    canonical.Href = AddKeyToUrl(canonicalUrl, pageKey, currentPage);
                }

                // rel prev
                if (currentPage > 1) {
                    string prevUrl;
                    if (currentPage == 2)
                    {
                        prevUrl = canonicalUrl;
                    }
                    else {
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
            else {
                // rel next
                // здесь всегда добавляется next, даже если второй страницы не существует
                _resourceManager.RegisterLink(new LinkEntry()
                {
                    Href = AddKeyToUrl(canonicalUrl, pageKey, 2),
                    Rel = "next"
                });
            }

            //var appPath = _workContext.Value.HttpContext.Request.ToApplicationRootUrlString();
        }

        private string AddKeyToUrl(string url, string key, int value) {
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

        public void ChangeMetaForList(int Page)
        {
            var currentPage = Page < 1 ? 1 : Page;

        }

        public string ChangeTitle(string title)
        {
            var pageKey = "page";
            var queryString = _workContext.Value.HttpContext.Request.QueryString;
            int currentPage;
            if (queryString[pageKey] == null || !Int32.TryParse(queryString[pageKey], out currentPage) || currentPage <= 1)
            {
                return title;
            }
            else
            {
                return string.Format(T("{0} Page {1}").ToString(), title, currentPage);
            }
        }
    }
}