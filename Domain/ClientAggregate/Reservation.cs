using Domain.ClientAggregate.Valueobjects;
using Domain.Common;
using Shared.Helpers;

namespace Domain.ClientAggregate
{
    public class Reservation : Entity
    {
        public int ClientId { get; private set; }
        public Name ClientName { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public string Date { get; private set; }
        public string Start { get; private set; }
        public string End { get; private set; }
        public double Price { get; private set; }
        public string CarrierCompany { get; private set; }

        public static Result<Reservation> Create(string Expires, int clientId, string clientFirstName, string clientLastName, string from, string to, string date, string start, string end, double price, string carrierCompany)
        {
            return Result.Success(new Reservation()
            {
                ClientId = clientId,
                ClientName = Name.Create(clientFirstName, clientLastName).Value,
                From = from,
                To = to,
                Date = date,
                Start = start,
                End = end,
                Price = price,
                CarrierCompany = carrierCompany
            });
        }
        private Reservation() { }
    }
}
