using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class OccupationDataDto
{
    /// <summary>
    /// Fecha específica del reporte de ocupación.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Porcentaje de ocupación en esa fecha.
    /// </summary>
    public double OccupiedPercentage { get; set; }

    /// <summary>
    /// Número de habitaciones ocupadas.
    /// </summary>
    public int OccupiedRooms { get; set; }

    /// <summary>
    /// Total de habitaciones disponibles en el alojamiento.
    /// </summary>
    public int TotalRooms { get; set; }
}