using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class ExtraServiceDTO
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

    // add reservation to allow the relationship with the Reservation entity
    public ICollection<Reservation>? Reservations { get; set; } = null!;

    // add availabilities to allow the relationship with the ServiceAvailability entity
    public ICollection<ServiceAvailability> Availabilities { get; set; } = new List<ServiceAvailability>();
}