using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostMaster.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ExtraServicesController : GenericController<ExtraService>
    {
        public ExtraServicesController(IGenericUnitOfWork<ExtraService> unitOfWork) : base(unitOfWork)
        {
        }
    }
}