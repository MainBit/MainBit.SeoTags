using Orchard.ContentManagement.MetaData.Models;

namespace MainBit.SeoTags.Settings
{
    public class SeoTagsPartSettings
    {
        public bool AddPageToTitle { get; set; }
        public bool FirstPageCanonical { get; set; }
        public bool UseTypePartDefSettings { get; set; }
    }

    public static class SeoTagsPartSettingsExtensions
    {
        public static SeoTagsPartSettings GetSeoTagsPartSettings(this ContentTypePartDefinition definition)
        {
            var settings = definition.Settings.GetModel<SeoTagsPartSettings>();
            if (settings.UseTypePartDefSettings == false)
            {
                return definition.PartDefinition.Settings.GetModel<SeoTagsPartSettings>();
            }
            return settings;
        }
    }

}