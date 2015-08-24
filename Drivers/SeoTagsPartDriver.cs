using MainBit.Utility.Services;
using MainBit.SeoTags.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Orchard.UI.Resources;
using Orchard.Utility.Extensions;
using System;
using System.Linq;
using MainBit.SeoTags.Settings;
using MainBit.SeoTags.Services;
using Orchard.ContentManagement.Aspects;

namespace MainBit.SeoTags.Drivers
{
    public class SeoTagsPartDriver : ContentPartDriver<SeoTagsPart>
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICurrentContentAccessor _currentContentAccessor;
        private readonly IResourceManager _resourceManager;
        private readonly ISeoTagsService _seoTagsService;

        public SeoTagsPartDriver(
            IWorkContextAccessor wca,
            ICurrentContentAccessor currentContentAccessor,
            IResourceManager resourceManager,
            ISeoTagsService seoTagsService)
        {
            _wca = wca;
            _currentContentAccessor = currentContentAccessor;
            _resourceManager = resourceManager;
            _seoTagsService = seoTagsService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(SeoTagsPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_SeoTags", (shape) =>
            {
                if (_currentContentAccessor.CurrentContentItem.Id != part.ContentItem.Id) { return null; }

                var workContext = _wca.GetContext();
                var settings = part.TypePartDefinition.GetSeoTagsPartSettings();

                var noindex = false;
                var pageKey = "page";
                var queryString = workContext.HttpContext.Request.QueryString;

                int currentPage = 0;

                // set robots
                if (queryString[pageKey] == null)
                {
                    currentPage = 0;
                    if (queryString.Count > 0) // contains a non-page parameter
                    {
                        noindex = true;
                    }
                }
                else if (!Int32.TryParse(queryString[pageKey], out currentPage) || currentPage <= 0) // contains invalid page parameter
                {
                    currentPage = 0;
                    noindex = true;
                }
                else if (queryString.Count > 1) // contains not only a page parameter
                {
                    noindex = true;
                }
                if (noindex)
                {
                    _resourceManager.SetMeta(new MetaEntry
                    {
                        Name = "robots",
                        Content = "noindex"
                    });
                }

                // set title
                var title = !String.IsNullOrWhiteSpace(part.Title)
                    ? part.Title
                    : part.Is<ITitleAspect>()
                        ? part.As<ITitleAspect>().Title
                        : null;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    workContext.Layout.Title = settings.AddPageToTitle
                            ? _seoTagsService.GetPageTitle(title, currentPage)
                            : title;
                }

                // set description
                if (!String.IsNullOrWhiteSpace(part.Description))
                {
                    _resourceManager.SetMeta(new MetaEntry
                    {
                        Name = "description",
                        Content = settings.AddPageToTitle
                            ? _seoTagsService.GetPageDescription(part.Description, currentPage)
                            : part.Description
                    });
                }

                // set keywords
                if (!String.IsNullOrWhiteSpace(part.Keywords))
                {
                    _resourceManager.SetMeta(new MetaEntry
                    {
                        Name = "keywords",
                        Content = part.Keywords
                    });
                }

                // set canonical
                var autoroutePart = part.As<Orchard.Autoroute.Models.AutoroutePart>();
                if (autoroutePart != null && !string.IsNullOrEmpty(autoroutePart.Path))
                {
                    var canonical = _resourceManager.GetRegisteredLinks().FirstOrDefault(p => p.Rel == "canonical");
                    if (canonical == null)
                    {
                        canonical = new LinkEntry()
                        {
                            Rel = "canonical"
                        };
                        _resourceManager.RegisterLink(canonical);
                    }

                    if (string.IsNullOrEmpty(part.Canonical))
                    {
                        canonical.Href = _wca.GetContext().HttpContext.Request.ToApplicationRootUrlString()
                             + "/" + autoroutePart.Path;
                    }
                    else
                    {
                        canonical.Href = _wca.GetContext().HttpContext.Request.ToApplicationRootUrlString()
                             + "/" + part.Canonical;
                    }
                }

                return null;
            });
        }

        //GET
        protected override DriverResult Editor(SeoTagsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_SeoTags_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/SeoTags",
                    Model: part,
                    Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(SeoTagsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            part.Title = part.Title.Trim();
            part.Description = part.Description.Trim();
            part.Keywords = part.Keywords.Trim();
            part.Canonical = part.Canonical.Trim();

            return Editor(part, shapeHelper);
        }

        protected override void Importing(SeoTagsPart part, ImportContentContext context)
        {
            var title = context.Attribute(part.PartDefinition.Name, "Title");
            if (title != null)
            {
                part.Title = title;

            }
            var canonical = context.Attribute(part.PartDefinition.Name, "Canonical");
            if (canonical != null)
            {
                part.Canonical = canonical;
            }

            var description = context.Attribute(part.PartDefinition.Name, "Description");
            if (description != null)
            {
                part.Description = description;
            }

            var keywords = context.Attribute(part.PartDefinition.Name, "Keywords");
            if (keywords != null)
            {
                part.Keywords = keywords;
            }
        }

        protected override void Exporting(SeoTagsPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Title", part.Title);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Canonical", part.Canonical);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Description", part.Description);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Keywords", part.Keywords);
        }
    }
}