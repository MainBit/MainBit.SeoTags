using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainBit.Common.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Html;

namespace MainBit.Common.Extensions
{
    [OrchardFeature("Opt.Classy")]
    public static class PartsExtension
    {
        public static bool PartsExists(this HtmlHelper html, string partName) {
            var currentContentAccessor = html.GetWorkContext().Resolve<ICurrentContentAccessor>();

            var contentItem = currentContentAccessor.CurrentContentItem;
            if (contentItem == null) {
                return false;
            }

            return contentItem.Parts.Any(p => p.PartDefinition.Name == partName);
        }
    }
}