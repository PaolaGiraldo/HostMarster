using HostMaster.Backend.Data;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Repositories.Implementations;

public class ReservationsRepository : GenericRepository<Reservation>, IReservationsRepository

{
    private readonly DataContext _context;

    public ReservationsRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Reservation>>> GetAsync()
    {
        {
            var reservation = await _context.Reservations
                 .Include(x => x.ReservationRooms)
                 .ToListAsync();

            if (reservation == null)
            {
                return new ActionResponse<IEnumerable<Reservation>>
                {
                    WasSuccess = false,
                    Message = "ERR001"
                };
            }

            return new ActionResponse<IEnumerable<Reservation>>
            {
                WasSuccess = true,
                Result = reservation
            };
        }
    }

    public Task<IEnumerable<Reservation>> GetComboAsync()
    {
        throw new NotImplementedException();
    }

    /*
    public async Task<IEnumerable<Reservation>> GetComboAsync()
    {
        return await _context.ReservationRooms.OrderBy(x => x.RoomId).ToListAsync();
    }*/
}