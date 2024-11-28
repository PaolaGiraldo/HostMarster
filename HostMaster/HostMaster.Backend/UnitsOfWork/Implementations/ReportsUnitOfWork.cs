using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class ReportsUnitOfWork : GenericUnitOfWork<OccupationDataDto>, IReportsUnitOfWork
{
    private readonly IReportsRepository _reportsRepository;

    public ReportsUnitOfWork(IGenericRepository<OccupationDataDto> repository, IReportsRepository reportsRepository) : base(repository)
    {
        _reportsRepository = reportsRepository;
    }

    public async Task<ActionResponse<IEnumerable<OccupationDataDto>>> GetOccupancyPercentageByAccommodationAsync(int accommodationId, DateTime startDate, DateTime endDate)
    => await _reportsRepository.GetOccupancyPercentageByAccommodationAsync(accommodationId, startDate, endDate);

    public async Task<ActionResponse<IEnumerable<MonthlyOccupancyDto>>> GetMonthlyOccupancyPercentageAsync(int accommodationId, DateTime startDate, DateTime endDate)
=> await _reportsRepository.GetMonthlyOccupancyPercentageAsync(accommodationId, startDate, endDate);

    public async Task<ActionResponse<IEnumerable<MonthlyRevenueDto>>> GetMonthlyRevenueAsync(int accommodationId, DateTime startDate, DateTime endDate)
=> await _reportsRepository.GetMonthlyRevenueAsync(accommodationId, startDate, endDate);
}