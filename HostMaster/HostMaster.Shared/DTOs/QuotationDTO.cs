using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class QuotationDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Name { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone(ErrorMessage = "El número de teléfono no es válido")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "El número de adultos es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El número de adultos debe ser al menos 1")]
    public int Adults { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El número de niños debe ser al menos 0")]
    public int Children { get; set; }

    [Required(ErrorMessage = "El tipo de habitación es obligatorio")]
    public string RoomType { get; set; }

    public string Observations { get; set; } // Este campo no es obligatorio
}