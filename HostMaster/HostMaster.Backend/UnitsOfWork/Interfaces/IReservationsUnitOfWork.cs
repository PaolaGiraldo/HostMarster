using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface IReservationsUnitOfWork
{
    Task<ActionResponse<Reservation>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Reservation>>> GetAsync();

    Task<IEnumerable<Reservation>> GetComboAsync();
}