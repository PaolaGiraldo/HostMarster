using HostMaster.Backend.UnitsOfWork.Implementations;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
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
        private readonly IExtraServicesUnitOfWork _extraServicesUnitOfWork;

        public ExtraServicesController(IGenericUnitOfWork<ExtraService> unitOfWork, IExtraServicesUnitOfWork extraServicesUnitOfWork) : base(unitOfWork)
        {
            _extraServicesUnitOfWork = extraServicesUnitOfWork;
        }

        [HttpGet("paginated")]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _extraServicesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _extraServicesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _extraServicesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpGet("totalRecordsPaginated")]
        public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _extraServicesUnitOfWork.GetTotalRecordsAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpPut("full")]
        public async Task<IActionResult> PutAsync(ExtraServiceDTO extraServiceDTO)
        {
            var action = await _extraServicesUnitOfWork.UpdateAsync(extraServiceDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }

        [HttpPost("availability")]
        public async Task<IActionResult> AddAvailabilityAsync(ServiceAvailabilityDTO availabilityDTO)
        {
            var response = await _extraServicesUnitOfWork.AddAvailabilityAsync(availabilityDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPut("availability")]
        public async Task<IActionResult> UpdateAvailabilityAsync(ServiceAvailabilityDTO availabilityDTO)
        {
            var response = await _extraServicesUnitOfWork.UpdateAvailabilityAsync(availabilityDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("{serviceId}/availabilities")]
        public async Task<IActionResult> GetAvailabilityAsync(int serviceId)
        {
            var response = await _extraServicesUnitOfWork.GetAvailabilityAsync(serviceId);
            Console.WriteLine(response.Result); // Log the response
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpGet("availabilities")]
        public async Task<IActionResult> GetAvailabilitiesAsync()
        {
            var response = await _extraServicesUnitOfWork.GetAvailabilitiesAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }
    }
}