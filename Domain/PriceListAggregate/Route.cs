namespace Domain.PriceListAggregate
{
    public class Route
    {
        public string Id { get; set; }
        public Location From { get; set; }
        public Location To { get; set; }
        public int Distance { get; set; }
        public List<Schedule> Schedule { get; set; }
    }
}