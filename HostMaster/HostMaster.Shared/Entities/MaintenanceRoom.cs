using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.Entities;

public class MaintenanceRoom
{
    public int Id { get; set; }
    public Maintenance Maintenance { get; set; } = null!;
    public int MaintenanceId { get; set; }
    public Room Room { get; set; } = null!;
    public int RoomId { get; set; }
}