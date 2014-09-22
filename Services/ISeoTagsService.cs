using Orchard;

namespace MainBit.SeoTags.Services
{
    public interface ISeoTagsService : IDependency
    {
        void RegisterMetaForList(
            int Page,
            int PageSize,
            double TotalItemCount,
            string PagerId);

        string GetTitle(string title);
    }
}