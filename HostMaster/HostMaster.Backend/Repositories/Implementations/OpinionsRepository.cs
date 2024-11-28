using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using static MudBlazor.Colors;

namespace HostMaster.Backend.Repositories.Implementations;

public class OpinionRepository(DataContext context) : GenericRepository<Opinion>(context), IOpinionsRepository

{
    private readonly DataContext _context = context;

    public override async Task<ActionResponse<IEnumerable<Opinion>>> GetAsync()
    {
        var opinions = await _context.Opinions
            .ToListAsync();

        return new ActionResponse<IEnumerable<Opinion>>
        {
            WasSuccess = true,
            Result = opinions
        };
    }

    public override async Task<ActionResponse<Opinion>> GetAsync(int opinionId)
    {
        var opinion = await _context.Opinions
           .FirstOrDefaultAsync(r => r.Id == opinionId);

        if (opinion == null)
        {
            return new ActionResponse<Opinion>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }
        return new ActionResponse<Opinion>
        {
            WasSuccess = true,
            Result = opinion
        };
    }

    public async Task<ActionResponse<Opinion>> AddAsync(OpinionDTO opinionDTO)
    {
        var opinion = new Opinion
        {
            Calification = opinionDTO.Calification,
            Comments = opinionDTO.Comments,
            Positives = opinionDTO.Like,
            Negatives = opinionDTO.Dislike,
            CreatedDate = DateTime.Now
        };

        _context.Add(opinion);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Opinion>
            {
                WasSuccess = true,
                Result = opinion
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Opinion>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Opinion>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public override async Task<ActionResponse<IEnumerable<Opinion>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Opinions
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Comments!.Equals(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Opinion>>
        {
            WasSuccess = true,
            Result = await queryable
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Opinions.AsQueryable();

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}