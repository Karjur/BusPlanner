using Domain.PriceListAggregate.ValueObjects;

namespace Domain.PriceListAggregate
{
    public class PriceList
    {
        public string Id { get; set; }
        public Expires Expires { get; set; }
        public List<Route> Routes { get; set; }
    }
}