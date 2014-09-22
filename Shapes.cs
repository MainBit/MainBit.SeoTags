using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.DisplayManagement.Descriptors;
using MainBit.Common.Services;
using MainBit.SeoTags.Models;
using MainBit.SeoTags.Services;
using System.Collections.Generic;
using Orchard.Environment;
using MainBit.SeoTags.Settings;
using Orchard;

namespace MainBit.SeoTags
{
    public class Shapes : IShapeTableProvider
    {
        private readonly Work<ICurrentContentAccessor> _currentContentAccessor;
        private readonly IWorkContextAccessor _wca;

        public Shapes(Work<ICurrentContentAccessor> currentContentAccessor,
            IWorkContextAccessor wca)
        {
            _currentContentAccessor = currentContentAccessor;
            _wca = wca;
        }

        public void Discover(ShapeTableBuilder builder)
        {


            builder.Describe("Pager")
                .OnDisplaying(displaying =>
                {
                    var _seoTagsService = _wca.GetContext().Resolve<ISeoTagsService>();
                    _seoTagsService.RegisterMetaForList(displaying.Shape.Page, displaying.Shape.PageSize, displaying.Shape.TotalItemCount, "");
                });

            builder.Describe("Layout")
                .OnDisplaying(displaying =>
                {
                    var seoTagsPart = _currentContentAccessor.Value.CurrentContentItem.As<SeoTagsPart>();
                    if (seoTagsPart == null) { return; }
                    var settings = seoTagsPart.TypePartDefinition.GetSeoTagsPartSettings();

                    if (seoTagsPart != null && string.IsNullOrEmpty(seoTagsPart.Title) == false)
                    {
                        displaying.Shape.Title = seoTagsPart.Title;
                    }
                    else
                    {
                        var titleAspect = _currentContentAccessor.Value.CurrentContentItem.As<ITitleAspect>();
                        if (titleAspect != null && string.IsNullOrEmpty(titleAspect.Title) == false)
                        {
                            displaying.Shape.Title = titleAspect.Title;
                        }
                    }

                    if (settings.AddPageToTitle)
                    {
                        var _seoTagsService = _wca.GetContext().Resolve<ISeoTagsService>();
                        displaying.Shape.Title = _seoTagsService.GetTitle(displaying.Shape.Title);
                    }
                });
        }
    }
}