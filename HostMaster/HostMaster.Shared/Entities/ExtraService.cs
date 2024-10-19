using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class ExtraService
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ServiceName { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string ServiceDescription { get; set; } = null!;

    public decimal Price { get; set; }

    // Relationships
    public ICollection<Reservation>? Reservations { get; set; }
}