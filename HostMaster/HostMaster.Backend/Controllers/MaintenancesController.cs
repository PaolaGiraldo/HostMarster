using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MudBlazor.CategoryTypes;

namespace HostMaster.Backend.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("/api/[controller]")]
public class MaintenancesController : GenericController<Maintenance>
{
    private readonly IMaintenancesUnitOfWork _maintenancesUnitOfWork;

    public MaintenancesController(IGenericUnitOfWork<Maintenance> unitOfWork, IMaintenancesUnitOfWork maintenancesUnitOfWork) : base(unitOfWork)
    {
        _maintenancesUnitOfWork = maintenancesUnitOfWork;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _maintenancesUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _maintenancesUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _maintenancesUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _maintenancesUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("combo/{roomId:int}")]
    public async Task<IActionResult> GetComboAsync(int roomId)
    {
        return Ok(await _maintenancesUnitOfWork.GetComboAsync(roomId));
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(MaintenanceDTO maintenanceDTO)
    {
        var action = await _maintenancesUnitOfWork.AddAsync(maintenanceDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PutAsync(MaintenanceDTO maintenanceDTO)
    {
        var action = await _maintenancesUnitOfWork.UpdateAsync(maintenanceDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("accommodation/{accommodationId:int}")]
    public async Task<IActionResult> GetByAccommodationIdAsync(int accommodationId)
    {
        var action = await _maintenancesUnitOfWork.GetByAccommodationIdAsync(accommodationId);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("room/{roomId:int}")]
    public async Task<IActionResult> GetByRoomIdAsync(int roomId)
    {
        return Ok(await _maintenancesUnitOfWork.GetByRoomIdAsync(roomId));
    }

    [HttpGet("date/{startDate:DateTime}")]
    public async Task<IActionResult> GetByStartDateAsync(DateTime startDate)
    {
        return Ok(await _maintenancesUnitOfWork.GetByStartDateAsync(startDate));
    }
}