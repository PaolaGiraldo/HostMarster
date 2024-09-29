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
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _reservationsUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _reservationsUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
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

    [HttpGet("accommodation/{accommodationId:int}")]
    public async Task<IActionResult> GetByAccommodationIdAsync(int accommodationId)
    {
        var action = await _reservationsUnitOfWork.GetByAccommodationIdAsync(accommodationId);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("room/{roomId:int}")]
    public async Task<IActionResult> GetByRoomIdAsync(int roomId)
    {
        return Ok(await _reservationsUnitOfWork.GetByRoomIdAsync(roomId));
    }

    [HttpGet("customer/{customerDocument:int}")]
    public async Task<IActionResult> GetByCustomerAsync(int customerDocument)
    {
        return Ok(await _reservationsUnitOfWork.GetByCustomerAsync(customerDocument));
    }

    [HttpGet("date/{startDate:DateTime}")]
    public async Task<IActionResult> GetByStartDateAsync(DateTime startDate)
    {
        return Ok(await _reservationsUnitOfWork.GetByStartDateAsync(startDate));
    }
}