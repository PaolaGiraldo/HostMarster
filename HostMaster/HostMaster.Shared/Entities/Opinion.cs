using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities;

public class Opinion
{
    public int Id { get; set; }

    [Display(Name = "Calification", ResourceType = typeof(Literals))]
    [Range(1, 5, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Calification { get; set; }

    [Display(Name = "Comments", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String? Comments { get; set; }

    [Display(Name = "Positives", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String? Positives { get; set; }

    [Display(Name = "Negatives", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String? Negatives { get; set; }

    public DateTime CreatedDate { get; set; }
}