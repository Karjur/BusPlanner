using Application.Common.Interfaces;
using Domain.ClientAggregate;
using Domain.ClientAggregate.Valueobjects;
using Domain.Common;
using Domain.PriceListAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class DataContext : DbContext, IDatabaseContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<PriceList> PriceLists { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : Entity
        {
            return Set<TEntity>();
        }
    }
}