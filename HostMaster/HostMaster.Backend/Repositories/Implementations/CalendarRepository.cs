using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Frontend.Pages.Calendar;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HostMaster.Backend.Repositories.Implementations;

public class CalendarRepository : GenericRepository<CalendarListDTO>, ICalendarRepository
{
    private readonly DataContext _context;

    public CalendarRepository(DataContext context) : base(context)
    {
        _context = context;
    }

 

    async Task<ActionResponse<IEnumerable<CalendarListDTO>>> ICalendarRepository.GetAsync(DateTime QueryDate)
{
    var queryDateOnly = QueryDate.Date;

    var calendarListDTOs = await GetQuerable(QueryDate).Select(r => new CalendarListDTO
         {
             Id = r.Id,
             StartDate = r.StartDate,
             EndDate = r.EndDate,
             NumberOfGuests = r.NumberOfGuests,
             ReservationState = r.ReservationState,
             RoomId = r.RoomId,
             RoomNumber = r.Room!.RoomNumber,
             AccommodationId = r.AccommodationId,
             CustomerDocument = r.CustomerDocumentNumber
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
            .Where(r => queryDateOnly >= r.StartDate.Date && queryDateOnly <= r.EndDate.Date)
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
                    CustomerDocument = r.CustomerDocumentNumber
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
}
