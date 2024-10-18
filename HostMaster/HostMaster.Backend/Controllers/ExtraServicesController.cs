using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HostMaster.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExtraServicesController : GenericController<ExtraService>
    {
        public ExtraServicesController(IGenericUnitOfWork<ExtraService> unitOfWork) : base(unitOfWork)
        {
        }
    }
}