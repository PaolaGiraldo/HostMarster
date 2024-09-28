using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class ReservationsUnitOfWork : GenericUnitOfWork<Reservation>, IReservationsUnitOfWork
{
    private readonly IReservationsRepository _reservationsRepository;

    public ReservationsUnitOfWork(IGenericRepository<Reservation> repository, IReservationsRepository reservationsRepository) : base(repository)
    {
        _reservationsRepository = reservationsRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Reservation>>> GetAsync()
    {
        return await _reservationsRepository.GetAsync();
    }

    public Task<IEnumerable<Reservation>> GetComboAsync()
    {
        throw new NotImplementedException();
    }
}