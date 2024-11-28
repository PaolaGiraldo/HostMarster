using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class MonthlyOccupancyDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int OccupiedRooms { get; set; }
    public int TotalDays { get; set; }
    public int TotalRooms { get; set; }
    public double OccupiedPercentage { get; set; }
}