using MainBit.SeoTags.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace MainBit.SeoTags.Handlers
{
    public class SeoTagsPartHandler : ContentHandler {

        public SeoTagsPartHandler(IRepository<SeoTagsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}