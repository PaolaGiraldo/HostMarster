using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class Reservation
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfGuests { get; set; }

    [Required]
    public string State { get; set; } = null!;

    public int CustomerId { get; set; }

    [Required]
    public Customer Customer { get; set; } = null!;

    public int AccommodationId { get; set; }

    [Required]
    public Accommodation Accommodation { get; set; } = null!;

    // Relationships
    public ICollection<Payment>? Payments { get; set; }

    //Y public ICollection<ExtraService>? ExtraServices { get; set; }

    public ICollection<ReservationRoom> ReservationRooms { get; set; } = null!;
}