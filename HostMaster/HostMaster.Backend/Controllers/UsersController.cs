using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : GenericController<User>
{
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public UsersController(IGenericUnitOfWork<User> unitOfWork, IUsersUnitOfWork usersUnitOfWork) : base(unitOfWork)
    {
        _usersUnitOfWork = usersUnitOfWork;
    }

    [HttpGet("full")]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _usersUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _usersUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(UserDTO userDTO)
    {
        var action = await _usersUnitOfWork.AddAsync(userDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}