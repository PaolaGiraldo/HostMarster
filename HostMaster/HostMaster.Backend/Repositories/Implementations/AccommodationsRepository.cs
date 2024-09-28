using HostMaster.Backend.Data;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Repositories.Implementations;

public class AccommodationsRepository : GenericRepository<Accommodation>, IAccommodationsRepository
{
	private readonly DataContext _dataContext;

	public AccommodationsRepository(DataContext context) : base(context)
	{
		_dataContext = context;
	}

	public override async Task<ActionResponse<IEnumerable<Accommodation>>> GetAsync()
	{
		var accommodations = await _dataContext.Accommodations
			.OrderBy(x => x.Name).ToListAsync();

		return new ActionResponse<IEnumerable<Accommodation>>
		{
			WasSuccess = true,
			Result = accommodations
		};
	}

	public override Task<ActionResponse<Accommodation>> AddAsync(Accommodation accommodation)
	{
		return base.AddAsync(accommodation);
	}
}