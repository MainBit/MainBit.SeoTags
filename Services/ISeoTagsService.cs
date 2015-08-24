using Orchard;

namespace MainBit.SeoTags.Services
{
    public interface ISeoTagsService : IDependency
    {
        void RegisterMetaForList(
            int page,
            int pageSize,
            double totalItemCount,
            string pagerId);

        string GetPageTitle(string title, int pageNumber);
        string GetPageDescription(string description, int pageNumber);
    }
}