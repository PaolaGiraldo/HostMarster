using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string Room { get; set; }

        [Required]
        public string StartDate { get; set; }

        [Required]
        public string EndDate { get; set; }
    }
}