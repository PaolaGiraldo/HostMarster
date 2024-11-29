using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface ICalendarUnitOfWork
{
    Task<ActionResponse<IEnumerable<CalendarListDTO>>> GetAsync(DateTime QueryDate);

    Task<ActionResponse<IEnumerable<CalendarListDTO>>> GetAsync(PaginationDTO pagination, DateTime QueryDate);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination, DateTime QueryDate);

    Task<ActionResponse<IEnumerable<Room>>> GetXAvailableRoomsAsync(DateTime QueryDate, int? accommodationId = null);
}