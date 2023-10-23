using Domain.PriceListAggregate;

namespace Web.Helpers
{
    public interface IFetchApi
    {
        Task<PriceList> FetchPriceListAsync();
    }
}