using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IReservationsRepository
{
    Task<ActionResponse<Reservation>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Reservation>>> GetAsync();

    Task<IEnumerable<Reservation>> GetComboAsync();
}