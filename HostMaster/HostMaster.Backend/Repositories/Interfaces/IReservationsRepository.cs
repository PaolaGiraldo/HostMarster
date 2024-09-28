using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IReservationsRepository
{
    Task<ActionResponse<Reservation>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Reservation>>> GetAsync();

    Task<ActionResponse<Reservation>> AddAsync(ReservationDTO reservationDTO);

    Task<ActionResponse<Reservation>> DeleteAsync(int id);

    Task<ActionResponse<Reservation>> UpdateAsync(ReservationDTO reservationDTO);

    Task<IEnumerable<Reservation>> GetComboAsync(int roomId);

    //Task<ActionResponse<IEnumerable<Reservation>>> GetAsync(PaginationDTO pagination);

    //Task<ActionResponse<Reservation>> GetTotalRecordsAsync(PaginationDTO pagination);
}