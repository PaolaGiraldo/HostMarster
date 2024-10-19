using Microsoft.EntityFrameworkCore;
using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using static MudBlazor.Colors;

namespace HostMaster.Backend.Repositories.Implementations;

public class StatesRepository : GenericRepository<State>, IStatesRepository
{
	private readonly DataContext _context;

	public StatesRepository(DataContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IEnumerable<State>> GetComboAsync(int countryId)
	{
		return await _context.States
			.Where(s => s.CountryId == countryId)
			.OrderBy(s => s.Name)
		.ToListAsync();
	}

	public override async Task<ActionResponse<State>> GetAsync(int id)
	{
		var state = await _context.States
			 .FirstOrDefaultAsync(s => s.Id == id);

		if (state == null)
		{
			return new ActionResponse<State>
			{
				WasSuccess = false,
				Message = "Estado no existe"
			};
		}

		return new ActionResponse<State>
		{
			WasSuccess = true,
			Result = state
		};
	}

	public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
	{
		var states = await _context.States
			.OrderBy(x => x.Name)
			.ToListAsync();
		return new ActionResponse<IEnumerable<State>>
		{
			WasSuccess = true,
			Result = states
		};
	}

	public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
	{
		var queryable = _context.States
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(pagination.Filter))
		{
			queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
		}

		return new ActionResponse<IEnumerable<State>>
		{
			WasSuccess = true,
			Result = await queryable
				.OrderBy(x => x.Name)
				.Paginate(pagination)
				.ToListAsync()
		};
	}

	public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
	{
		var queryable = _context.States
			.Where(x => x.Country!.Id == pagination.Id)
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(pagination.Filter))
		{
			queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
		}

		double count = await queryable.CountAsync();
		int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
		return new ActionResponse<int>
		{
			WasSuccess = true,
			Result = totalPages
		};
	}

	async Task<ActionResponse<int>> IStatesRepository.GetTotalRecordsAsync(PaginationDTO pagination)
	{
		var queryable = _context.States.AsQueryable();

		if (!string.IsNullOrWhiteSpace(pagination.Filter))
		{
			queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
		}

		double count = await queryable.CountAsync();
		return new ActionResponse<int>
		{
			WasSuccess = true,
			Result = (int)count
		};
	}

	async Task<ActionResponse<State>> IStatesRepository.AddAsync(StateDTO stateDTO)
	{
		var country = await _context.Countries.FindAsync(stateDTO.CountryId);
		if (country == null)
		{
			return new ActionResponse<State>
			{
				WasSuccess = false,
				Message = "ERR004"
			};
		}

		var state = new State
		{
			Country = country,
			Name = stateDTO.Name,
		};

		_context.Add(state);
		try
		{
			await _context.SaveChangesAsync();
			return new ActionResponse<State>
			{
				WasSuccess = true,
				Result = state
			};
		}
		catch (DbUpdateException)
		{
			return new ActionResponse<State>
			{
				WasSuccess = false,
				Message = "ERR003"
			};
		}
		catch (Exception exception)
		{
			return new ActionResponse<State>
			{
				WasSuccess = false,
				Message = exception.Message
			};
		}
	}
}