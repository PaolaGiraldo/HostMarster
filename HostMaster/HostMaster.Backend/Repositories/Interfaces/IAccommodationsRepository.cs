using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IAccommodationsRepository
{
	Task<ActionResponse<Accommodation>> GetAsync(int id);

	Task<ActionResponse<IEnumerable<Accommodation>>> GetAsync();

	Task<ActionResponse<IEnumerable<Accommodation>>> GetAsync(PaginationDTO pagination);

	Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
}