using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostMaster.Shared.Entities;

public class Employee : User
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    [Display(Name = "Department", ResourceType = typeof(Literals))]
    public string Department { get; set; } = null!;

    [Required]
    [Display(Name = "HireDate", ResourceType = typeof(Literals))]
    public DateTime HireDate { get; set; }

    // Foreign keys
    public int AccommodationId { get; set; }

    [Required]
    public Accommodation Accommodation { get; set; } = null!;
}