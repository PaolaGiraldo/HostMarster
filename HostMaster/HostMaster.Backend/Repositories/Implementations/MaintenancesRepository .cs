using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using static MudBlazor.Colors;

namespace HostMaster.Backend.Repositories.Implementations;

public class MaintenancesRepository(DataContext context) : GenericRepository<Maintenance>(context), IMaitenancesRepository

{
    private readonly DataContext _context = context;

    public override async Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync()
    {
        var maintenances = await _context.Maintenances
            .Include(x => x.Room)
            .OrderBy(x => x.RoomId)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Maintenance>>
        {
            WasSuccess = true,
            Result = maintenances
        };
    }

    public override async Task<ActionResponse<Maintenance>> GetAsync(int maintenanceId)
    {
        var maintenance = await _context.Maintenances
            .Include(r => r.Room)
           .FirstOrDefaultAsync(r => r.Id == maintenanceId);

        if (maintenance == null)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }
        return new ActionResponse<Maintenance>
        {
            WasSuccess = true,
            Result = maintenance
        };
    }

    public async Task<ActionResponse<Maintenance>> AddAsync(MaintenanceDTO maintenanceDTO)
    {
        var room = await _context.Rooms.FindAsync(maintenanceDTO.RoomId);
        if (room == null)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = "ERR_RES001"
            };
        }
        else if (!room.IsAvailable)
        {
            return new ActionResponse<Maintenance>
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

        var maintenance = new Maintenance
        {
            StartDate = maintenanceDTO.StartDate ?? DateTime.Now,
            EndDate = maintenanceDTO.EndDate ?? DateTime.Now,
            RoomId = maintenanceDTO.RoomId,
            AccommodationId = maintenanceDTO.AccommodationId,
            Observations = maintenanceDTO.Observations
        };

        _context.Add(maintenance);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Maintenance>
            {
                WasSuccess = true,
                Result = maintenance
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<Maintenance>> UpdateAsync(MaintenanceDTO maintenanceDTO)
    {
        var currentMaintenance = await _context.Maintenances.FindAsync(maintenanceDTO.Id);
        if (currentMaintenance == null)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = "ERR_RES003"
            };
        }

        var room = await _context.Rooms.FindAsync(maintenanceDTO.RoomId);
        if (room == null)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = "ERR_RES001"
            };
        }
        else if (!room.IsAvailable)
        {
            return new ActionResponse<Maintenance>
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

        currentMaintenance.StartDate = maintenanceDTO.StartDate ?? DateTime.Now;
        currentMaintenance.EndDate = maintenanceDTO.EndDate ?? DateTime.Now;
        currentMaintenance.RoomId = maintenanceDTO.RoomId;
        currentMaintenance.AccommodationId = maintenanceDTO.AccommodationId;
        currentMaintenance.Observations = maintenanceDTO.Observations;

        _context.Update(currentMaintenance);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Maintenance>
            {
                WasSuccess = true,
                Result = currentMaintenance
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Maintenance>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<Maintenance>> GetComboAsync(int roomId)
    {
        return await _context.Maintenances
                    .Where(x => x.RoomId == roomId)
                    .OrderBy(x => x.Room.RoomNumber)
                    .ToListAsync();
    }

    public async Task<ActionResponse<IEnumerable<Maintenance>>> GetByAccommodationIdAsync(int accommodatioId)
    {
        var maintenances = await _context.Maintenances
       .Where(r => r.AccommodationId == accommodatioId)
       .ToListAsync();

        return new ActionResponse<IEnumerable<Maintenance>>
        {
            WasSuccess = true,
            Result = maintenances
        };
    }

    public async Task<IEnumerable<Maintenance>> GetByRoomIdAsync(int roomId)
    {
        return await _context.Maintenances
         .Where(r => r.RoomId == roomId)

         .ToListAsync();
    }

    public async Task<IEnumerable<Maintenance>> GetByStartDateAsync(DateTime startDate)
    {
        return await _context.Maintenances
        .Where(r => r.StartDate == startDate)
        .ToListAsync();
    }

    public override async Task<ActionResponse<IEnumerable<Maintenance>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Maintenances
            .Include(x => x.Room)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Room!.Id.Equals(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Maintenance>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.RoomId)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Maintenances.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.RoomId.Equals(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}