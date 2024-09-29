using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class AccommodationsUnitOfWork : GenericUnitOfWork<Accommodation>, IAccommodationsUnitOfWork
{
	private readonly IAccommodationsRepository _accommodationsRepository;

	public AccommodationsUnitOfWork(IGenericRepository<Accommodation> repository, IAccommodationsRepository accommodationsRepository) : base(repository)
	{
		_accommodationsRepository = accommodationsRepository;
	}
}