using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.Entities;

public class ReservationRoom
{
    public int id { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int ReservationId { get; set; }
    public Room Room { get; set; } = null!;
    public int RoomId { get; set; }
}