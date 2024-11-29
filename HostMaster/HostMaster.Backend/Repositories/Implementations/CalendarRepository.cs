using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Frontend.Pages.Calendar;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HostMaster.Backend.Repositories.Implementations;

public class CalendarRepository : GenericRepository<CalendarListDTO>, ICalendarRepository
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CalendarRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    async Task<ActionResponse<IEnumerable<CalendarListDTO>>> ICalendarRepository.GetAsync(DateTime QueryDate)
    {
        var queryDateOnly = QueryDate.Date;

        var calendarListDTOs = await GetQuerable(QueryDate).Select(static r => new CalendarListDTO
        {
            Id = r.Id,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            NumberOfGuests = r.NumberOfGuests,
            ReservationState = r.ReservationState,
            RoomId = r.RoomId,
            RoomNumber = r.Room!.RoomNumber,
            AccommodationId = r.AccommodationId,
            CustomerDocument = r.CustomerDocumentNumber,
            Room = r.Room
        })
                .OrderBy(x => x.RoomNumber)
                .ToListAsync();

        return new ActionResponse<IEnumerable<CalendarListDTO>>
        {
            WasSuccess = true,
            Result = calendarListDTOs
        };
    }

    public IOrderedQueryable<Reservation> GetQuerable(DateTime QueryDate)
    {
        var queryDateOnly = QueryDate.Date;

        return _context.Reservations
            .Include(x => x.Room)
            .Include(x => x.Customer)
            .Where(r => queryDateOnly >= r.StartDate.Date && queryDateOnly <= r.EndDate.Date && (r.ReservationState == "Confirmed" || r.ReservationState == "Pending"))
            .OrderBy(x => x.RoomId);
    }

    async Task<ActionResponse<IEnumerable<CalendarListDTO>>> ICalendarRepository.GetAsync(PaginationDTO pagination, DateTime queryDate)
    {
        var queryable = GetQuerable(queryDate)
                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Room!.RoomNumber.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<CalendarListDTO>>
        {
            WasSuccess = true,
            Result = await queryable
                .Select(r => new CalendarListDTO
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    NumberOfGuests = r.NumberOfGuests,
                    ReservationState = r.ReservationState,
                    RoomId = r.RoomId,
                    RoomNumber = r.Room!.RoomNumber,
                    AccommodationId = r.AccommodationId,
                    CustomerDocument = r.CustomerDocumentNumber,
                    FullName = r.Customer.FirstName + " " + r.Customer.LastName,
                })
                .OrderBy(x => x.RoomNumber)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    async Task<ActionResponse<int>> ICalendarRepository.GetTotalRecordsAsync(PaginationDTO pagination, DateTime queryDate)
    {
        var queryable = GetQuerable(queryDate).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Room!.RoomNumber.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<IEnumerable<Room>>> GetXAvailableRoomsAsync(DateTime date, int? accommodationId = null)
    {
        var dateParam = date.Date;
        int actualAccommodationId = accommodationId ?? 1;

        // Prefiltrar las reservas según las condiciones del ON
        var filteredReservations = _context.Reservations
            .Where(res => res.StartDate >= dateParam
                          && (res.ReservationState == "Confirmed" || res.ReservationState == "Pending")
                          && res.AccommodationId == actualAccommodationId);

        // Realizar el LEFT JOIN con la inclusión de RoomType
        var availableRooms = await _context.Rooms
            .Include(r => r.RoomType) // Incluir la entidad RoomType
            .Where(r => r.AccommodationId == actualAccommodationId)
            .GroupJoin(
                filteredReservations,
                room => room.Id,
                res => res.RoomId,
                (room, resGroup) => new { room, resGroup }
            )
            .SelectMany(
                x => x.resGroup.DefaultIfEmpty(),
                (x, res) => new { x.room, res }
            )
            .Where(x => x.res == null)
            .Select(x => x.room)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Room>>
        {
            WasSuccess = true,
            Result = availableRooms
        };
    }

    private int GetAccommodationIdFromUser()
    {
        var user = _httpContextAccessor.HttpContext.User;
        var accommodationIdClaim = user.FindFirst("AccommodationId");

        if (accommodationIdClaim != null && int.TryParse(accommodationIdClaim.Value, out int accommodationId))
        {
            return accommodationId;
        }

        throw new Exception("No se pudo obtener el AccommodationId del usuario.");
    }
}