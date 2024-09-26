using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class Customer : User
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "PhoneNumber", ResourceType = typeof(Literals))]
    public string PhoneNumber { get; set; } = null!;

    // Relationships
    public ICollection<Reservation>? Reservations { get; set; }
}