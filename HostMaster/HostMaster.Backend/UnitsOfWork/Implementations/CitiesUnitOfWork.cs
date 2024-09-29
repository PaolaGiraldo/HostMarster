using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class CitiesUnitOfWork : GenericUnitOfWork<City>, ICitiesUnitOfWork
{
	private readonly ICitiesRepository _citiesRepository;

	public CitiesUnitOfWork(IGenericRepository<City> repository, ICitiesRepository citiesRepository) : base(repository)
	{
		_citiesRepository = citiesRepository;
	}

	public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination) => await _citiesRepository.GetAsync(pagination);

	public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _citiesRepository.GetTotalPagesAsync(pagination);

	public async Task<IEnumerable<City>> GetComboAsync(int stateId) => await _citiesRepository.GetComboAsync(stateId);
}