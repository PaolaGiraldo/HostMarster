using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class ExtraService
{
	public int Id { get; set; }

	[Display(Name = "Service", ResourceType = typeof(Literals))]
	[MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
	[Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
	public string ServiceName { get; set; } = null!;

	[Display(Name = "Description", ResourceType = typeof(Literals))]
	[MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
	[Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
	public string ServiceDescription { get; set; } = null!;

	[Display(Name = "Price", ResourceType = typeof(Literals))]
	public decimal Price { get; set; }

	// Relationships
	public ICollection<Reservation>? Reservations { get; set; }

	public ICollection<ServiceAvailability>? Availabilities { get; set; }
}