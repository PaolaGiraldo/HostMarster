using System.ComponentModel.DataAnnotations;

namespace HostMaster.Shared.Entities
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }
    }
}