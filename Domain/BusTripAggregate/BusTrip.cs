using Domain.BusRouteAggregate;

namespace Domain.BusTripAggregate
{
    public class BusTrip
    {
        public string Id { get; private set; }
        public double Price { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public string Carrier { get; private set; }
        public BusRoute BusRoute { get; private set; }
        private BusTrip() { }

        public void Create(BusRoute busRoute, string id, double price , DateTime start, DateTime end, string carrier)
        {
            BusRoute = busRoute;
            Id = id;
            Price = price;
            Start = start;
            End = end;
            Carrier = carrier;
        }
    }
}
