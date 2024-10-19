using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface ICountriesUnitOfWork
{
	Task<ActionResponse<Country>> GetAsync(int id);

	Task<ActionResponse<IEnumerable<Country>>> GetAsync();

	Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);

	Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

	Task<IEnumerable<Country>> GetComboAsync();

	Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}