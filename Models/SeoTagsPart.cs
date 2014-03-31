using Orchard.ContentManagement;

namespace MainBit.SeoTags.Models
{
    public class SeoTagsPart : ContentPart<SeoTagsPartRecord>
    {
        public string Title
        {
            get { return Retrieve(x => x.Title); }
            set { Store(x => x.Title, value); }
        }

        public string Keywords
        {
            get { return Retrieve(x => x.Keywords); }
            set { Store(r => r.Keywords, value); }
        }

        public string Description
        {
            get { return Retrieve(x => x.Description); }
            set { Store(x => x.Description, value); }
        }

        public string Canonical
        {
            get { return Retrieve(x => x.Canonical); }
            set { Store(x => x.Canonical, value); }
        }
    }
}