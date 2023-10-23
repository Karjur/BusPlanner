using Domain.BusTripAggregate;

namespace Domain.BusRouteAggregate
{
    public class BusRoute
    {
        private readonly HashSet<BusTrip> _busTrips = null!;
        public IReadOnlyCollection<BusTrip> BusTrips => _busTrips;
        public string Id { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        private BusRoute() { }
        public void Create(string id, string from, string to)
        {
            Id = id;
            From = from;
            To = to;
        }

        public void AddTripToSchedule(BusTrip busTrip)
        {
            _busTrips.Add(busTrip);
        }
    }
}
