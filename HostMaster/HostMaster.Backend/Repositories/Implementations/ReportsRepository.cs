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

    public async Task<ActionResponse<IEnumerable<MonthlyOccupancyDto>>> GetMonthlyOccupancyPercentageAsync(int accommodationId, DateTime startDate, DateTime endDate)
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

        // Generar datos de ocupación diaria
        var dailyOccupancy = reservations
            .SelectMany(reservation =>
                Enumerable.Range(0, 1 + (reservation.EndDate - reservation.StartDate).Days)
                          .Select(offset => reservation.StartDate.AddDays(offset))
                          .Where(date => date >= startDate && date <= endDate)
                          .Select(date => new { Date = date, RoomId = reservation.RoomId }))
            .GroupBy(x => x.Date)
            .ToList();

        // Agrupar por mes
        var monthlyOccupancy = dailyOccupancy
            .GroupBy(d => new { d.Key.Year, d.Key.Month })
            .Select(group => new MonthlyOccupancyDto
            {
                Year = group.Key.Year,
                Month = group.Key.Month,
                OccupiedRooms = group.Sum(day => day.Select(x => x.RoomId).Distinct().Count()),
                TotalDays = group.Count(),
                TotalRooms = totalRooms,
                OccupiedPercentage = totalRooms > 0 ? (double)group.Sum(day => day.Select(x => x.RoomId).Distinct().Count()) / (totalRooms * group.Count()) * 100 : 0
            })
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToList();

        // Devolver el resultado en un ActionResponse
        return new ActionResponse<IEnumerable<MonthlyOccupancyDto>>
        {
            WasSuccess = true,
            Result = monthlyOccupancy
        };
    }

    public async Task<ActionResponse<IEnumerable<MonthlyRevenueDto>>> GetMonthlyRevenueAsync(int accommodationId, DateTime startDate, DateTime endDate)
    {
        // Obtener las reservas dentro del rango de fechas para el alojamiento especificado
        var reservations = await _context.Reservations
            .Include(r => r.Room)
            .ThenInclude(room => room.RoomType)
            .Where(res => res.AccommodationId == accommodationId &&
                          res.StartDate <= endDate &&
                          res.EndDate >= startDate)
            .ToListAsync();

        // Validar que existan reservas
        if (!reservations.Any())
        {
            return new ActionResponse<IEnumerable<MonthlyRevenueDto>>
            {
                WasSuccess = false,
                Message = "No se encontraron reservas en el rango de fechas especificado."
            };
        }

        // Calcular ingresos diarios y agruparlos por mes
        var monthlyRevenue = reservations
            .SelectMany(reservation =>
                Enumerable.Range(0, 1 + (reservation.EndDate - reservation.StartDate).Days)
                          .Select(offset => new
                          {
                              Date = reservation.StartDate.AddDays(offset),
                              DailyRevenue = reservation.Room?.RoomType?.Price ?? 0
                          }))
            .Where(x => x.Date >= startDate && x.Date <= endDate)
            .GroupBy(x => new { x.Date.Year, x.Date.Month })
            .Select(group => new MonthlyRevenueDto
            {
                Year = group.Key.Year,
                Month = group.Key.Month,
                TotalRevenue = group.Sum(x => x.DailyRevenue)
            })
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToList();

        return new ActionResponse<IEnumerable<MonthlyRevenueDto>>
        {
            WasSuccess = true,
            Result = monthlyRevenue
        };
    }
}