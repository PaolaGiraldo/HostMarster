using HostMaster.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)

{
    public DbSet<Accommodation> Accommodations { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ExtraService> ExtraServices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomInventoryItem> RoomInventoryItems { get; set; }
    public DbSet<RoomPhoto> RoomPhotos { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<ReservationRoom> ReservationRooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Room>().HasIndex(x => new { x.AccommodationId, x.RoomNumber }).IsUnique();
        modelBuilder.Entity<Reservation>().HasIndex(x => new { x.RoomId, x.AccommodationId, x.StartDate, x.EndDate }).IsUnique();
        //modelBuilder.Entity<ReservationRoom>().HasIndex(x => new { x.ReservationId, x.RoomId }).IsUnique();

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>().HasIndex(u => u.Document).IsUnique();
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Customer>().HasIndex(c => c.Document).IsUnique();
        modelBuilder.Entity<Employee>().ToTable("Employees");

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>().HasIndex(u => u.Document).IsUnique();
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Employee>().ToTable("Employees");

        DisableCascadingDelete(modelBuilder);
    }

    private void DisableCascadingDelete(ModelBuilder modelBuilder)
    {
        var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var relationship in relationships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}