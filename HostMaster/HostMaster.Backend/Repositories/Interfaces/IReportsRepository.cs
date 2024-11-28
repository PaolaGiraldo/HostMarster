using HostMaster.Shared.DTOs;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IReportsRepository
{
    Task<ActionResponse<IEnumerable<OccupationDataDto>>> GetOccupancyPercentageByAccommodationAsync(int accommodationId, DateTime startDate, DateTime endDate);

    Task<ActionResponse<IEnumerable<MonthlyOccupancyDto>>> GetMonthlyOccupancyPercentageAsync(int accommodationId, DateTime startDate, DateTime endDate);

    Task<ActionResponse<IEnumerable<MonthlyRevenueDto>>> GetMonthlyRevenueAsync(int accommodationId, DateTime startDate, DateTime endDate);
}