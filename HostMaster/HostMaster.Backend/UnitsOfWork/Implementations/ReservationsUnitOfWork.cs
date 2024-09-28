using HostMaster.Backend.Repositories.Implementations;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
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

    public override async Task<ActionResponse<IEnumerable<Reservation>>> GetAsync() => await _reservationsRepository.GetAsync();

    public override async Task<ActionResponse<Reservation>> GetAsync(int id) => await _reservationsRepository.GetAsync(id);

    public async Task<ActionResponse<Reservation>> AddAsync(ReservationDTO reservationDTO) => await _reservationsRepository.AddAsync(reservationDTO);

    public async Task<IEnumerable<Reservation>> GetComboAsync(int roomId) => await _reservationsRepository.GetComboAsync(roomId);

    public async Task<ActionResponse<Reservation>> UpdateAsync(ReservationDTO reservationDTO) => await _reservationsRepository.UpdateAsync(reservationDTO);
}