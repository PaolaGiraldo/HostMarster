using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class OpinionDTO
{
    public int Id { get; set; }

    [Display(Name = "Calification", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
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