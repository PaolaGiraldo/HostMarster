using HostMaster.Backend.Data;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using static MudBlazor.Colors;

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
        var reservations = await _context.Reservations
            .Include(r => r.ReservationRooms)
            .OrderBy(r => r.StartDate)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Reservation>>
        {
            WasSuccess = true,
            Result = reservations
        };
    }

    public override async Task<ActionResponse<Reservation>> GetAsync(int id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.RoomId)

           .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation == null)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }
        return new ActionResponse<Reservation>
        {
            WasSuccess = true,
            Result = reservation
        };
    }

    public async Task<ActionResponse<Reservation>> AddAsync(ReservationDTO reservationDTO)
    {
        var room = await _context.Reservations.FindAsync(reservationDTO.RoomId);
        if (room == null)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES001"
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

    public async Task<ActionResponse<Reservation>> UpdateAsync(ReservationDTO reservationDTO)
    {
        var currentReservation = await _context.Reservations.FindAsync(reservationDTO.Id);
        if (currentReservation == null)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES003"
            };
        }

        var room = await _context.Reservations.FindAsync(reservationDTO.RoomId);
        if (room == null)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES001"
            };
        }

        currentReservation.StartDate = reservationDTO.StartDate;
        currentReservation.EndDate = reservationDTO.EndDate;
        currentReservation.RoomId = reservationDTO.RoomId;
        currentReservation.NumberOfGuests = reservationDTO.NumberOfGuests;

        _context.Update(currentReservation);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Reservation>
            {
                WasSuccess = true,
                Result = currentReservation
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

    public async Task<IEnumerable<Reservation>> GetComboAsync(int roomId)
    {
        return await _context.Reservations
                    .Where(x => x.RoomId == roomId)
                    .OrderBy(x => x.Room.RoomNumber)
                    .ToListAsync();
    }
}