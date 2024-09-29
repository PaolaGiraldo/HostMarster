using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccommodationsController : GenericController<Accommodation>
{
	public AccommodationsController(IGenericUnitOfWork<Accommodation> unitOfWork) : base(unitOfWork)
	{
	}
}