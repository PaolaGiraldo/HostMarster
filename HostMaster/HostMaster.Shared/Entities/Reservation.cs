using HostMaster.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [Display(Name = "Room", ResourceType = typeof(Literals))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
        public string Room { get; set; } = null!;

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }
    }
}