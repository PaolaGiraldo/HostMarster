using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class Reservation
{
    public int Id { get; set; }

    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime StartDate { get; set; }

    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime EndDate { get; set; }

    [Display(Name = "NumberOfGuests", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int NumberOfGuests { get; set; }

    [Display(Name = "ReservationState", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string ReservationState { get; set; } = null!;

    //Foreign keys
    [Display(Name = "RoomId", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int RoomId { get; set; }

    public Room? Room { get; set; }

    [Display(Name = "Customer", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    [Display(Name = "AccommodationId", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int AccommodationId { get; set; }

    public Accommodation? Accommodation { get; set; }

    // Relationships
    public ICollection<Payment>? Payments { get; set; }

    public ICollection<ExtraService>? ExtraServices { get; set; }

    public ICollection<ReservationRoom>? ReservationRooms { get; set; } = null!;
}