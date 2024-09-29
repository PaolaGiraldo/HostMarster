using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class ReservationDTO
{
    public int Id { get; set; }

    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime StartDate { get; set; }

    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime EndDate { get; set; }

    [Display(Name = "NumberOfGuests", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int NumberOfGuests { get; set; }

    [Display(Name = "ReservationState", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string ReservationState { get; set; } = null!;

    //Foreign keys
    [Display(Name = "RoomId", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int RoomId { get; set; }

    [Display(Name = "AccommodationId", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int AccommodationId { get; set; }

    public int CustomerDocumentNumber { get; set; }
}