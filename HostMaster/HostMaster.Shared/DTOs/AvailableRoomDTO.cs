using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class AvailableRoomDTO
{
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
}