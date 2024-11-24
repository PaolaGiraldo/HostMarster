using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Interfaces
{
	public interface IExtraServicesUnitOfWork
	{
		//Task<ActionResponse<IEnumerable<ExtraService>>> GetByIdAsync(int Id);

		Task<ActionResponse<ExtraService>> AddAsync(ExtraServiceDTO extraServiceDTO);

		Task<ActionResponse<ExtraService>> GetAsync(int id);

		Task<ActionResponse<IEnumerable<ExtraService>>> GetAsync();

		Task<IEnumerable<ExtraService>> GetComboAsync();

		Task<ActionResponse<IEnumerable<ExtraService>>> GetAsync(PaginationDTO pagination);

		Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

		Task<ActionResponse<ExtraService>> UpdateAsync(ExtraServiceDTO extraServiceDTO);

		Task<ActionResponse<ServiceAvailability>> AddAvailabilityAsync(ServiceAvailabilityDTO availabilityDTO);

		Task<ActionResponse<ServiceAvailability>> UpdateAvailabilityAsync(ServiceAvailabilityDTO availabilityDTO);

		Task<ActionResponse<ServiceAvailability>> GetAvailabilityAsync(int serviceId);

		Task<ActionResponse<IEnumerable<ServiceAvailability>>> GetAvailabilitiesAsync();
	}
}