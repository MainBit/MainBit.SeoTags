using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.DisplayManagement.Descriptors;
using MainBit.Utility.Services;
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
        private readonly Work<ISeoTagsService> _seoTagsService;

        public Shapes(Work<ICurrentContentAccessor> currentContentAccessor,
            Work<ISeoTagsService> seoTagsService)
        {
            _currentContentAccessor = currentContentAccessor;
            _seoTagsService = seoTagsService;
        }

        public void Discover(ShapeTableBuilder builder)
        {


            builder.Describe("Pager")
                .OnDisplayed(displaying =>
                {
                    _seoTagsService.Value.RegisterMetaForList(displaying.Shape.Page, displaying.Shape.PageSize, displaying.Shape.TotalItemCount, "");
                });
        }
    }
}