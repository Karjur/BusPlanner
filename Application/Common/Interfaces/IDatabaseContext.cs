using Domain.ClientAggregate;
using Domain.PriceListAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IDatabaseContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Location> Locations { get; set; }

        public int SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : Domain.Common.Entity;
    }
}
