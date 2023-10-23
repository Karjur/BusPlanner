using Domain.ClientAggregate.Valueobjects;
using Domain.Common;
using Shared.Helpers;

namespace Domain.ClientAggregate
{
    public class Client : Entity
    {
        private HashSet<Reservation> _reservations = new();
        public IReadOnlyCollection<Reservation> Reservations => _reservations;
        public Name Name { get; private set; } = null!;
        public static Result<Client> Create(Name name)
        {
            return Result.Success(new Client()
            {
                Name = name
            });
        }

        public Result AddReservation(DateTime expiresAt, Reservation reservation)
        {
            if (expiresAt < DateTime.Now)
            {
                return Result.Failure("Hinnakiri on aegunud.");
            }
            _reservations.Add(reservation);
            return Result.Success();
        }

        private Client() { }
    }
}
