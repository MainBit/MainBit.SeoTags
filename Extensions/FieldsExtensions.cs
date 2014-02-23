using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MainBit.Common.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Html;
using HtmlHelper = System.Web.Mvc.HtmlHelper;
using Orchard.ContentManagement.FieldStorage;

namespace MainBit.Common.Extensions
{
    [OrchardFeature("Opt.Classy")]
    public static class FieldsExtensions
    {
        public static string GetEnumerationFieldValue(this HtmlHelper html, params string[] fieldNames) {
            
            var value = new List<string>();

            var currentContentAccessor = html.GetWorkContext().Resolve<ICurrentContentAccessor>();
            var contentItem = currentContentAccessor.CurrentContentItem;
            if (contentItem == null) {
                return "";
            }

            foreach (var fieldName in fieldNames)
            {
                var isFind = false;
                foreach (var part in contentItem.Parts)
                {
                    foreach (var field in part.Fields)
                    {
                        if (field.Name == fieldName) {
                            var currentValue = field.Storage.Get<string>();
                            if (!string.IsNullOrEmpty(currentValue)) {
                                value.Add(currentValue);
                            }
                            break;
                        }
                    }
                    if (isFind)
                    {
                        break;
                    }
                }
            }
            return html.Encode(string.Join(" ", value));
        }
    }
}