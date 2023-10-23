using Microsoft.EntityFrameworkCore;
using Domain.BusRouteAggregate;
using Domain.BusTripAggregate;

namespace Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<BusRoute> BusRoutes { get; set; } = null!;
        public DbSet<BusTrip> BusTrips { get; set; } = null!;
    }
}
