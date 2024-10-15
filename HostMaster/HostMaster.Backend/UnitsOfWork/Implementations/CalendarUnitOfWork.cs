using HostMaster.Backend.Repositories.Implementations;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class CalendarUnitOfWork : GenericUnitOfWork<CalendarListDTO>, ICalendarUnitOfWork
{
    private readonly ICalendarRepository _calendarRepository;

    public CalendarUnitOfWork(IGenericRepository<CalendarListDTO> repository, ICalendarRepository calendarRepository) : base(repository)
    {
        _calendarRepository = calendarRepository;
    }

    public async Task<ActionResponse<IEnumerable<CalendarListDTO>>> GetAsync(DateTime QueryDate) => await _calendarRepository.GetAsync(QueryDate);

    async Task<ActionResponse<IEnumerable<CalendarListDTO>>> ICalendarUnitOfWork.GetAsync(PaginationDTO pagination, DateTime QueryDate) => await _calendarRepository.GetAsync(pagination, QueryDate);

    async Task<ActionResponse<int>> ICalendarUnitOfWork.GetTotalRecordsAsync(PaginationDTO pagination, DateTime QueryDate) => await _calendarRepository.GetTotalRecordsAsync(pagination, QueryDate);
}
