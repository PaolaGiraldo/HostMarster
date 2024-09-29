using HostMaster.Shared.Enums;
using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "DocumentType", ResourceType = typeof(Literals))]
    public string DocumentType { get; set; } = null!;

    [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Document", ResourceType = typeof(Literals))]
    public string Document { get; set; } = null!;

    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "FirstName", ResourceType = typeof(Literals))]
    public string FirstName { get; set; } = null!;

    [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Display(Name = "LastName", ResourceType = typeof(Literals))]
    public string LastName { get; set; } = null!;

    [Display(Name = "Photo", ResourceType = typeof(Literals))]
    public string? Photo { get; set; } = null!;

    public string? PhoneNumber { get; set; } = null!;

    [Display(Name = "UserType", ResourceType = typeof(Literals))]
    public UserType UserType { get; set; } = UserType.Admin;

    public string FullName => $"{FirstName} {LastName}";
}