using HostMaster.Backend.Data;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Repositories.Implementations;

public class ReportsRepository : GenericRepository<OccupationDataDto>, IReportsRepository
{
    private readonly DataContext _context;

    public ReportsRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ActionResponse<IEnumerable<OccupationDataDto>>> GetOccupancyPercentageByAccommodationAsync(int accommodationId, DateTime startDate, DateTime endDate)
    {
        // Obtener habitaciones del alojamiento
        var rooms = await _context.Rooms
            .Where(r => r.AccommodationId == accommodationId)
            .ToListAsync();

        // Obtener reservas dentro del rango de fechas
        var reservations = await _context.Reservations
            .Where(res => res.AccommodationId == accommodationId &&
                          res.StartDate <= endDate &&
                          res.EndDate >= startDate)
            .ToListAsync();

        // Calcular el total de habitaciones
        var totalRooms = rooms.Count;

        // Calcular ocupación en memoria
        var occupancyData = reservations
            .SelectMany(reservation =>
                Enumerable.Range(0, 1 + (reservation.EndDate - reservation.StartDate).Days)
                          .Select(offset => reservation.StartDate.AddDays(offset))
                          .Where(date => date >= startDate && date <= endDate)
                          .Select(date => new { Date = date, RoomId = reservation.RoomId }))
            .GroupBy(x => x.Date)
            .Select(group => new OccupationDataDto
            {
                Date = group.Key,
                OccupiedRooms = group.Select(x => x.RoomId).Distinct().Count(),
                TotalRooms = totalRooms,
                OccupiedPercentage = totalRooms > 0 ? (double)group.Select(x => x.RoomId).Distinct().Count() / totalRooms * 100 : 0
            })
            .OrderBy(x => x.Date)
            .ToList();

        // Devolver el resultado en un ActionResponse
        return new ActionResponse<IEnumerable<OccupationDataDto>>
        {
            WasSuccess = true,
            Result = occupancyData
        };
    }

}
