using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class ServiceAvailabilityDTO
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
}