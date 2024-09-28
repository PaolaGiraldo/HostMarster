using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using static MudBlazor.Colors;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IReservationsRepository
{
    Task<IEnumerable<Reservation>> GetComboAsync(int roomId);

    Task<ActionResponse<Reservation>> AddAsync(ReservationDTO reservationDTO);

    Task<ActionResponse<Reservation>> UpdateAsync(ReservationDTO reservationDTO);

    Task<ActionResponse<Reservation>> GetAsync(int id);
}