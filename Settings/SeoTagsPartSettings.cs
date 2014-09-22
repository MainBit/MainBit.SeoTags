using Orchard.ContentManagement.MetaData.Models;

namespace MainBit.SeoTags.Settings
{
    public class SeoTagsPartSettings
    {
        public bool AddPageToTitle { get; set; }
    }

    public static class SeoTagsPartSettingsExtensions
    {
        public static SeoTagsPartSettings GetSeoTagsPartSettings(this ContentTypePartDefinition definition)
        {
            //var settings = definition.Settings.GetModel<SeoTagsPartSettings>();
            //if (settings.UsePartDefSettings == true)
            //{
            //    return definition.PartDefinition.Settings.GetModel<SeoTagsPartSettings>();
            //}
            //return settings;

            return definition.PartDefinition.Settings.GetModel<SeoTagsPartSettings>();
        }
    }
}