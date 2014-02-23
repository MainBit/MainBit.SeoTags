using Orchard.ContentManagement;

namespace MainBit.SeoTags.Models
{
    public class SeoTagsPart : ContentPart<SeoTagsPartRecord>
    {
        public string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }

        public string Keywords
        {
            get { return Record.Keywords; }
            set { Record.Keywords = value; }
        }

        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }

        public string Canonical
        {
            get { return Record.Canonical; }
            set { Record.Canonical = value; }
        }
    }
}