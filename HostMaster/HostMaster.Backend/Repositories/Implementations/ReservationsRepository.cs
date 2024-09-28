using HostMaster.Backend.Data;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.DTOs;
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

    /*
    public override async Task<ActionResponse<IEnumerable<Reservation>>> GetAsync()
    {
    }

    public override async Task<ActionResponse<Reservation>> GetAsync(int id)
    {
    }*/

    public async Task<ActionResponse<Reservation>> AddAsync(ReservationDTO reservationDTO)
    {
        var room = await _context.Rooms.FindAsync(reservationDTO.RoomId);
        if (room == null)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        var reservation = new Reservation
        {
            StartDate = reservationDTO.StartDate,
            EndDate = reservationDTO.EndDate,
            ReservationState = reservationDTO.ReservationState,
            RoomId = reservationDTO.RoomId,
            NumberOfGuests = reservationDTO.NumberOfGuests,
            CustomerId = reservationDTO.CustomerDocumentNumber,
        };

        _context.Add(reservation);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Reservation>
            {
                WasSuccess = true,
                Result = reservation
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    /*
    public async Task<IEnumerable<Reservation>> GetComboAsync(int roomId)
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResponse<Reservation>> UpdateAsync(ReservationDTO reservationDTO)
    {
        throw new NotImplementedException();
    }*/
}