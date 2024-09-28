using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReservationsController : GenericController<Reservation>
{
    private readonly IReservationsUnitOfWork _reservationsUnitOfWork;

    public ReservationsController(IGenericUnitOfWork<Reservation> unitOfWork, IReservationsUnitOfWork reservationsUnitOfWork) : base(unitOfWork)
    {
        _reservationsUnitOfWork = reservationsUnitOfWork;
    }

    [HttpGet]
    public override Task<IActionResult> GetAsync()
    {
        return base.GetAsync();
    }

    [HttpGet("{id}")]
    public override Task<IActionResult> GetAsync(int id)
    {
        return base.GetAsync(id);
    }

    [HttpGet("combo/{roomId:int}")]
    public async Task<IActionResult> GetComboAsync(int roomId)
    {
        return Ok(await _reservationsUnitOfWork.GetComboAsync(roomId));
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(ReservationDTO reservationDTO)
    {
        var action = await _reservationsUnitOfWork.AddAsync(reservationDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}