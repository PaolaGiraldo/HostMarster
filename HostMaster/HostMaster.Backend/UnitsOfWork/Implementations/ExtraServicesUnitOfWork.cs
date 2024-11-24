using HostMaster.Backend.Repositories.Implementations;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations
{
	public class ExtraServicesUnitOfWork : GenericUnitOfWork<ExtraService>, IExtraServicesUnitOfWork
	{
		private readonly IExtraServicesRepository _extraServicesRepository;

		public ExtraServicesUnitOfWork(IGenericRepository<ExtraService> repository, IExtraServicesRepository extraServicesRepository) : base(repository)
		{
			_extraServicesRepository = extraServicesRepository;
		}

		public async Task<ActionResponse<ExtraService>> AddAsync(ExtraServiceDTO extraServiceDTO) => await _extraServicesRepository.AddAsync(extraServiceDTO);

		public override async Task<ActionResponse<IEnumerable<ExtraService>>> GetAsync() => await _extraServicesRepository.GetAsync();

		public override async Task<ActionResponse<ExtraService>> GetAsync(int id) => await _extraServicesRepository.GetAsync(id);

		public override async Task<ActionResponse<IEnumerable<ExtraService>>> GetAsync(PaginationDTO pagination) => await _extraServicesRepository.GetAsync(pagination);

		public async Task<IEnumerable<ExtraService>> GetComboAsync() => await _extraServicesRepository.GetComboAsync();

		public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _extraServicesRepository.GetTotalRecordsAsync(pagination);

		public async Task<ActionResponse<ExtraService>> UpdateAsync(ExtraServiceDTO extraServiceDTO) => await _extraServicesRepository.UpdateAsync(extraServiceDTO);

		public async Task<ActionResponse<ServiceAvailability>> AddAvailabilityAsync(ServiceAvailabilityDTO availabilityDTO) => await _extraServicesRepository.AddAvailabilityAsync(availabilityDTO);

		public async Task<ActionResponse<ServiceAvailability>> UpdateAvailabilityAsync(ServiceAvailabilityDTO availabilityDTO) => await _extraServicesRepository.UpdateAvailabilityAsync(availabilityDTO);

		public async Task<ActionResponse<ServiceAvailability>> GetAvailabilityAsync(int serviceId) => await _extraServicesRepository.GetAvailabilityAsync(serviceId);

		public async Task<ActionResponse<IEnumerable<ServiceAvailability>>> GetAvailabilitiesAsync() => await _extraServicesRepository.GetAvailabilitiesAsync();
	}
}