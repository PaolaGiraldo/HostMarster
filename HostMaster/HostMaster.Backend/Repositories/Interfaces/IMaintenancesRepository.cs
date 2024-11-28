using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using static MudBlazor.Colors;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IMaitenanceRepository
{
    Task<ActionResponse<Maintenance>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync();

    Task<ActionResponse<Maintenance>> AddAsync(MaintenanceDTO maintenanceDTO);

    Task<ActionResponse<Maintenance>> DeleteAsync(int id);

    Task<ActionResponse<Maintenance>> UpdateAsync(MaintenanceDTO maintenanceDTO);

    Task<IEnumerable<Maintenance>> GetComboAsync(int maintenanceId);

    Task<ActionResponse<IEnumerable<Maintenance>>> GetByAccommodationIdAsync(int accommodationId);

    Task<IEnumerable<Maintenance>> GetByRoomIdAsync(int roomId);

    Task<IEnumerable<Maintenance>> GetByStartDateAsync(DateTime startDate);

    Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}