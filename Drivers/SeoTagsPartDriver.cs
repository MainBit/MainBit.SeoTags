using MainBit.Common.Services;
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

namespace MainBit.SeoTags.Drivers
{
    public class SeoTagsPartDriver : ContentPartDriver<SeoTagsPart>
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICurrentContentAccessor _currentContentAccessor;
        private readonly IResourceManager _resourceManager;

        public SeoTagsPartDriver(
            IWorkContextAccessor workContextAccessor,
            ICurrentContentAccessor currentContentAccessor,
            IResourceManager resourceManager)
        {
            _wca = workContextAccessor;
            _currentContentAccessor = currentContentAccessor;
            _resourceManager = resourceManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override DriverResult Display(SeoTagsPart part, string displayType, dynamic shapeHelper)
        {
            if (_currentContentAccessor.CurrentContentItem.Id != part.ContentItem.Id) { return null; }
            var noindex = false;
            var pageKey = "page";
            var queryString = _wca.GetContext().HttpContext.Request.QueryString;
            int currentPage;
            if(queryString[pageKey] == null) {
                currentPage = 0;
                if (queryString.Count > 0)
                {
                    noindex = true;
                }
            }
            else if (!Int32.TryParse(queryString[pageKey], out currentPage))
            {
                currentPage = 0;
                noindex = true;
            }
            else if (_wca.GetContext().HttpContext.Request.RawUrl.Contains('&'))
            {
                noindex = true;
            }

            if (noindex) {
                _resourceManager.SetMeta(new MetaEntry
                {
                    Name = "robots",
                    Content = "noindex"
                });
            }

            if (!String.IsNullOrWhiteSpace(part.Description))
            {
                _resourceManager.SetMeta(new MetaEntry
                {
                    Name = "description",
                    Content = part.Description
                    //Content = currentPage > 1 ?  string.Format(T("{0} Page {1}").ToString(), part.Description, currentPage) : part.Description
                });
            }
            if (!String.IsNullOrWhiteSpace(part.Keywords))
            {
                _resourceManager.SetMeta(new MetaEntry
                {
                    Name = "keywords",
                    Content = part.Keywords
                    //Content = currentPage > 1 ? string.Format(T("{0}, Page {1}").ToString(), part.Keywords, currentPage) : part.Keywords
                });
            }

            var autoroutePart = part.ContentItem.As<Orchard.Autoroute.Models.AutoroutePart>();
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