using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using static MudBlazor.Colors;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface IReservationsUnitOfWork
{
    Task<IEnumerable<Reservation>> GetComboAsync(int reservationId);

    Task<ActionResponse<Reservation>> AddAsync(ReservationDTO reservationDTO);

    Task<ActionResponse<Reservation>> UpdateAsync(ReservationDTO reservationDTO);

    Task<ActionResponse<IEnumerable<Reservation>>> GetAsync();

    Task<ActionResponse<Reservation>> GetAsync(int reservationId);

    Task<ActionResponse<IEnumerable<Reservation>>> GetByAccommodationIdAsync(int accommodationId);

    Task<IEnumerable<Reservation>> GetByRoomIdAsync(int roomId);

    Task<IEnumerable<Reservation>> GetByCustomerAsync(int customerDocument);

    Task<IEnumerable<Reservation>> GetByStartDateAsync(DateTime startDate);

    Task<ActionResponse<IEnumerable<Reservation>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
}