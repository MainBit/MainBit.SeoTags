using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.DisplayManagement.Descriptors;
using MainBit.Common.Services;
using MainBit.SeoTags.Models;
using MainBit.SeoTags.Services;
using System.Collections.Generic;
using Orchard.Environment;

namespace MainBit.SeoTags {
    public class Shapes : IShapeTableProvider {
        private readonly Work<ICurrentContentAccessor> _currentContentAccessor;

        public Shapes(Work<ICurrentContentAccessor> currentContentAccessor)
        {
            _currentContentAccessor = currentContentAccessor;
        }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Pager")
                .OnDisplaying(displaying => {
                    //_seoTagsService.RegisterRelForList(displaying.Shape.Page, displaying.Shape.PageSize, displaying.Shape.TotalItemCount, "");
                });

            builder.Describe("Layout")
                .OnDisplaying(displaying =>
                {
                    var seoTagsPart = _currentContentAccessor.Value.CurrentContentItem.As<SeoTagsPart>();
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
                    
                });
        }
    }
}