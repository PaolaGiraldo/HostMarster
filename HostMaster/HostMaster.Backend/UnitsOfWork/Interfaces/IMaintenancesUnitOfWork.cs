using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using System.Threading.Tasks;
using static MudBlazor.Colors;
using System.Collections.Generic;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface IMaintenancesUnitOfWork
{
    Task<IEnumerable<Maintenance>> GetComboAsync(int maintenanceId);

    Task<ActionResponse<Maintenance>> AddAsync(MaintenanceDTO MaintenanceDTO);

    Task<ActionResponse<Maintenance>> UpdateAsync(MaintenanceDTO reservationDTO);

    Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync();

    Task<ActionResponse<Maintenance>> GetAsync(int reservationId);

    Task<ActionResponse<IEnumerable<Maintenance>>> GetByAccommodationIdAsync(int accommodationId);

    Task<IEnumerable<Maintenance>> GetByRoomIdAsync(int roomId);

    Task<IEnumerable<Maintenance>> GetByStartDateAsync(DateTime startDate);

    Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}