using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class Customer : User
{
    public int Id { get; set; }

    // Relationships
    public ICollection<Reservation>? Reservations { get; set; }
}