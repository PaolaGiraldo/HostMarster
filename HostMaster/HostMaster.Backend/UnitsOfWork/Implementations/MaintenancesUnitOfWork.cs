using HostMaster.Backend.Repositories.Implementations;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class MaintenancesUnitOfWork(IGenericRepository<Maintenance> repository, IMaitenancesRepository maintenancesRepository) : GenericUnitOfWork<Maintenance>(repository), IMaintenancesUnitOfWork
{
    private readonly IMaitenancesRepository _maintenancesRepository = maintenancesRepository;

    public override async Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync() => await _maintenancesRepository.GetAsync();

    public override async Task<ActionResponse<Maintenance>> GetAsync(int id) => await _maintenancesRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync(PaginationDTO pagination) => await _maintenancesRepository.GetAsync(pagination);

    public async Task<ActionResponse<Maintenance>> AddAsync(MaintenanceDTO maintenanceDTO) => await _maintenancesRepository.AddAsync(maintenanceDTO);

    public async Task<IEnumerable<Maintenance>> GetComboAsync(int roomId) => await _maintenancesRepository.GetComboAsync(roomId);

    public async Task<ActionResponse<Maintenance>> UpdateAsync(MaintenanceDTO maintenanceDTO) => await _maintenancesRepository.UpdateAsync(maintenanceDTO);

    public async Task<ActionResponse<IEnumerable<Maintenance>>> GetByAccommodationIdAsync(int accommodationId) => await _maintenancesRepository.GetByAccommodationIdAsync(accommodationId);

    public async Task<IEnumerable<Maintenance>> GetByRoomIdAsync(int roomId) => await _maintenancesRepository.GetByRoomIdAsync(roomId);

    public async Task<IEnumerable<Maintenance>> GetByStartDateAsync(DateTime startDate) => await _maintenancesRepository.GetByStartDateAsync(startDate);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _maintenancesRepository.GetTotalRecordsAsync(pagination);
}