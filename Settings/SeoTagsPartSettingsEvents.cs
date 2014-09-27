using System.Collections.Generic;
using System.Globalization;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Localization;

namespace MainBit.SeoTags.Settings
{
    public class SeoTagsPartSettingsEvents : ContentDefinitionEditorEventsBase
    {

        public Localizer T { get; set; }

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "SeoTagsPart")
                yield break;

            var model = definition.Settings.GetModel<SeoTagsPartSettings>();
            yield return DefinitionTemplate(model);
        }
        public override IEnumerable<TemplateViewModel> PartEditor(ContentPartDefinition definition)
        {
            if (definition.Name != "SeoTagsPart")
                yield break;

            var settings = definition.Settings.GetModel<SeoTagsPartSettings>();

            yield return DefinitionTemplate(settings);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "SeoTagsPart")
                yield break;

            var settings = new SeoTagsPartSettings();
            if (updateModel.TryUpdateModel(settings, "SeoTagsPartSettings", null, null))
            {
                builder.WithSetting("SeoTagsPartSettings.AddPageToTitle", settings.AddPageToTitle.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("SeoTagsPartSettings.FirstPageCanonical", settings.AddPageToTitle.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("SeoTagsPartSettings.UseTypePartDefSettings", settings.AddPageToTitle.ToString(CultureInfo.InvariantCulture));
            }

            yield return DefinitionTemplate(settings);
        }
        public override IEnumerable<TemplateViewModel> PartEditorUpdate(ContentPartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "SeoTagsPart")
                yield break;

            var settings = new SeoTagsPartSettings
            {
            };

            if (updateModel.TryUpdateModel(settings, "SeoTagsPartSettings", null, null))
            {
                builder.WithSetting("SeoTagsPartSettings.AddPageToTitle", settings.AddPageToTitle.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("SeoTagsPartSettings.FirstPageCanonical", settings.AddPageToTitle.ToString(CultureInfo.InvariantCulture));
            }

            yield return DefinitionTemplate(settings);
        }
    }
}
