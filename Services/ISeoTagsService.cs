using Orchard;

namespace MainBit.SeoTags.Services
{
    public interface ISeoTagsService : IDependency {
        void RegisterRelForList(
            int Page,
            int PageSize,
            double TotalItemCount,
            string PagerId);
    }
}