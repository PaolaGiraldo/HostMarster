using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Frontend.Repositories;
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
            .Include(x => x.Room)
            .Include(x => x.Customer)
            .OrderBy(x => x.RoomId)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Reservation>>
        {
            WasSuccess = true,
            Result = reservations
        };
    }

    public override async Task<ActionResponse<Reservation>> GetAsync(int reservationId)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Room)
           .FirstOrDefaultAsync(r => r.Id == reservationId);

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
        var room = await _context.Rooms.FindAsync(reservationDTO.RoomId);
        if (room == null)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES001"
            };
        }
        else if (!room.IsAvailable)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES007"
            };
        }
        else if (room.RoomType.MaxGuests <= reservationDTO.NumberOfGuests)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES005"
            };
        }

        /* if (reservationDTO.StartDate <= reservationDTO.EndDate)
         {
             return new ActionResponse<Reservation>
             {
                 WasSuccess = false,
                 Message = "ERR_RES004"
             };
         }*/

        if (!await IsRoomAvailableForReservationAsync(reservationDTO.RoomId, reservationDTO.StartDate, reservationDTO.EndDate))
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES006"
            };
        }

        var extraServices = CreateExtraServices(reservationDTO.ExtraServices);

        var reservation = new Reservation
        {
            StartDate = reservationDTO.StartDate ?? DateTime.Today,
            EndDate = reservationDTO.EndDate ?? DateTime.Today.AddDays(3),
            ReservationState = reservationDTO.ReservationState,
            RoomId = reservationDTO.RoomId,
            NumberOfGuests = reservationDTO.NumberOfGuests,
            CustomerDocumentNumber = reservationDTO.CustomerDocument,
            AccommodationId = reservationDTO.AccommodationId,
            ExtraServices = await extraServices
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

        var room = await _context.Rooms.FindAsync(reservationDTO.RoomId);
        if (room == null)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES001"
            };
        }
        else if (!room.IsAvailable)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES005"
            };
        }

        /*if (reservationDTO.StartDate <= reservationDTO.EndDate)
        {
            return new ActionResponse<Reservation>
            {
                WasSuccess = false,
                Message = "ERR_RES004"
            };
        }*/

        currentReservation.StartDate = reservationDTO.StartDate ?? DateTime.Now;
        currentReservation.EndDate = reservationDTO.EndDate ?? DateTime.Now;
        currentReservation.RoomId = reservationDTO.RoomId;
        currentReservation.NumberOfGuests = reservationDTO.NumberOfGuests;
        currentReservation.AccommodationId = reservationDTO.AccommodationId;
        currentReservation.CustomerDocumentNumber = reservationDTO.CustomerDocument;
        currentReservation.ReservationState = reservationDTO.ReservationState;
        //currentReservation.ExtraServices = reservationDTO.ExtraServices;

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

    public async Task<ActionResponse<IEnumerable<Reservation>>> GetByAccommodationIdAsync(int accommodatioId)
    {
        var reservations = await _context.Reservations
       .Where(r => r.AccommodationId == accommodatioId)
       .ToListAsync();

        return new ActionResponse<IEnumerable<Reservation>>
        {
            WasSuccess = true,
            Result = reservations
        };
    }

    public async Task<IEnumerable<Reservation>> GetByRoomIdAsync(int roomId)
    {
        return await _context.Reservations
         .Where(r => r.RoomId == roomId)

         .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByCustomerAsync(int customerDocument)
    {
        return await _context.Reservations
        .Where(r => r.CustomerDocumentNumber == customerDocument)
        .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByStartDateAsync(DateTime startDate)
    {
        return await _context.Reservations
        .Where(r => r.StartDate == startDate)
        .ToListAsync();
    }

    public override async Task<ActionResponse<IEnumerable<Reservation>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Reservations
            .Include(x => x.Room)
            .ThenInclude(x => x!.RoomType)
            .Include(x => x.Customer)
            .Include(x => x.Accommodation)
            .Include(x => x.ExtraServices)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Customer!.FirstName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Reservation>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.RoomId)
                .Paginate(pagination)
                .Select(x => new Reservation
                {
                    Id = x.Id,
                    RoomId = x.RoomId,
                    CustomerDocumentNumber = x.CustomerDocumentNumber,
                    ReservationState = x.ReservationState,
                    NumberOfGuests = x.NumberOfGuests,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Room = new Room
                    {
                        Id = x.Room!.Id,
                        RoomNumber = x.Room.RoomNumber,
                        RoomType = x.Room.RoomType,
                    },
                    Customer = new Customer
                    {
                        Id = x.Customer.Id,
                        FirstName = x.Customer.FirstName,
                        LastName = x.Customer.LastName,
                        Email = x.Customer.Email,
                        PhoneNumber = x.Customer.PhoneNumber
                    },
                    Accommodation = new Accommodation
                    {
                        Id = x.AccommodationId,
                        Name = x.Accommodation!.Name
                    },
                    ExtraServices = x.ExtraServices
                })
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Reservations.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Customer!.FirstName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<bool> IsRoomAvailableForReservationAsync(int roomId, DateTime? startDate, DateTime? endDate)
    {
        // Verificar si hay reservas superpuestas
        var overlappingReservation = await _context.Reservations
            .Where(r => r.RoomId == roomId)
            .AnyAsync(r =>
                (startDate < r.EndDate && endDate > r.StartDate)); // Verifica superposición de fechas

        if (overlappingReservation)
        {
            return false; // Hay una reserva superpuesta
        }

        // Verificar si hay mantenimientos superpuestos
        var overlappingMaintenance = await _context.Maintenances
            .Where(m => m.RoomId == roomId)
            .AnyAsync(m =>
                (startDate < m.EndDate && endDate > m.StartDate)); // Verifica superposición de fechas

        return !overlappingMaintenance; // Retorna verdadero si no hay mantenimiento superpuesto
    }

    public async Task<List<ExtraService>> CreateExtraServices(IEnumerable<String> extraServices)
    {
        var reservationServices = new List<ExtraService>();

        foreach (var serviceId in extraServices)
        {
            var service = _context.ExtraServices
                .Where(s => s.ServiceName == serviceId)
                .FirstAsync();

            reservationServices.Add(service.Result);
        }

        return reservationServices;
    }
}