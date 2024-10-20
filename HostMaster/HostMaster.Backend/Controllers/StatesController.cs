using Microsoft.AspNetCore.Mvc;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using HostMaster.Backend.UnitsOfWork.Implementations;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class StatesController : GenericController<State>
{
    private readonly IStatesUnitOfWork _statesUnitOfWork;

    public StatesController(IGenericUnitOfWork<State> unitOfWork, IStatesUnitOfWork statesUnitOfWork) : base(unitOfWork)
    {
        _statesUnitOfWork = statesUnitOfWork;
    }

    [HttpGet("combo/{countryId:int}")]
    public async Task<IActionResult> GetComboAsync(int countryId)
    {
        return Ok(await _statesUnitOfWork.GetComboAsync(countryId));
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _statesUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(StateDTO stateDTO)
    {
        var action = await _statesUnitOfWork.AddAsync(stateDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _statesUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _statesUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _statesUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }
}