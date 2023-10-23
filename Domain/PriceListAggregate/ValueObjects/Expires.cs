using Domain.Common;

namespace Domain.PriceListAggregate.ValueObjects
{
    public class Expires : ValueObject
    {
        public string Date { get; set; }
        public int TimezoneType { get; set; }
        public string Timezone { get; set; }
    }

}
