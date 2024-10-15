using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface ICalendarRepository
{
    Task<ActionResponse<IEnumerable<CalendarListDTO>>> GetAsync(DateTime queryDate);

    Task<ActionResponse<IEnumerable<CalendarListDTO>>> GetAsync(PaginationDTO pagination, DateTime queryDate);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination, DateTime queryDate);
}
