using MainBit.SeoTags.Models;
using Orchard.Alias;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using WebAdvanced.Sitemap.Services;

namespace MainBit.SeoTags.Sitemap
{
    public class CanonicalSitemapRouteFilter : ISitemapRouteFilter
    {
        private readonly IAliasService _aliasService;
        private readonly IContentManager _contentManager;

        public CanonicalSitemapRouteFilter(
            IAliasService aliasService,
            IContentManager contentManager)
        {
            _aliasService = aliasService;
            _contentManager = contentManager;
        }



        private List<string> _canonicals = null;
        private List<string> canonicals
        {
            get
            {
                if (_canonicals == null)
                {
                    _canonicals = _contentManager.Query<SeoTagsPart>(VersionOptions.Published)
                        .Where<SeoTagsPartRecord>(p => p.Canonical != null && p.Canonical != string.Empty)
                        .List()
                        .Select(p => p.As<AutoroutePart>().Path)
                        .ToList();
                }
                return _canonicals;
            }
        }

        public bool AllowUrl(string path)
        {
            return canonicals.Contains(path) == false;
        }
    }
}