using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface IReservationsUnitOfWork
{
    Task<IEnumerable<Reservation>> GetComboAsync(int roomId);

    Task<ActionResponse<Reservation>> AddAsync(ReservationDTO reservationDTO);

    Task<ActionResponse<Reservation>> UpdateAsync(ReservationDTO reservationDTO);

    Task<ActionResponse<Reservation>> GetAsync(int id);
}