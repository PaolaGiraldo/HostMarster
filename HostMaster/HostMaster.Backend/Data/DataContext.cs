using HostMaster.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Data
{
    public class DataContext : DbContext

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>().HasIndex(x => x.Number).IsUnique();
        }
    }
}