using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostMaster.Shared.Entities;

public class Employee
{
    public int Id { get; set; }

    [MaxLength(15)]
    [Required]
    public string DocumentId { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string LastName { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string Position { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string Email { get; set; } = null!;

    [MaxLength(15)]
    [Required]
    public string PhoneNumber { get; set; } = null!;

    public string? Photo { get; set; }

    // Foreign keys
    public int AccommodationId { get; set; }

    [Required]
    public Accommodation Accommodation { get; set; } = null!;
}