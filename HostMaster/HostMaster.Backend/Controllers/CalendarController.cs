using HostMaster.Backend.UnitsOfWork.Implementations;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalendarController : GenericController<CalendarListDTO>
{
    private readonly ICalendarUnitOfWork _calendarUnitOfWork;

    public CalendarController(IGenericUnitOfWork<CalendarListDTO> unitOfWork, ICalendarUnitOfWork calendarUnitOfWork) : base(unitOfWork)
    {
        _calendarUnitOfWork = calendarUnitOfWork;
    }

    [HttpGet("bydate")]
    public async Task<IActionResult> GetAsync([FromQuery] DateTime queryDate)
    {
        //if (string.isnullorempty(querydate))
        //{
        //    return badrequest("la fecha es requerida.");
        //}

        //datetime querydatetime;
        //try
        //{
        //    string format = "dd/mm/yyyy";
        //    querydatetime = datetime.parseexact(querydate, format, cultureinfo.invariantculture);
        //}
        //catch (formatexception)
        //{
        //    return badrequest("el formato de la fecha es inválido. utiliza 'dd/mm/yyyy'.");
        //}

        var response = await _calendarUnitOfWork.GetAsync(queryDate);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpGet("paginatedbydate")]
    public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination, DateTime queryDate)
    {
        //if (string.IsNullOrEmpty(queryDate))
        //{
        //    return BadRequest("La fecha es requerida.");
        //}

        //DateTime queryDateTime;
        //try
        //{
        //    string format = "dd/MM/yyyy";
        //    queryDateTime = DateTime.ParseExact(queryDate, format, CultureInfo.InvariantCulture);
        //}
        //catch (FormatException)
        //{
        //    return BadRequest("El formato de la fecha es inválido. Utiliza 'dd/MM/yyyy'.");
        //}

        var response = await _calendarUnitOfWork.GetAsync(pagination, queryDate);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginatedbydate")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination, DateTime queryDate)
    {
        //if (string.IsNullOrEmpty(queryDate))
        //{
        //    return BadRequest("La fecha es requerida.");
        //}

        //DateTime queryDateTime;
        //try
        //{
        //    string format = "dd/MM/yyyy";
        //    queryDateTime = DateTime.ParseExact(queryDate, format, CultureInfo.InvariantCulture);
        //}
        //catch (FormatException)
        //{
        //    return BadRequest("El formato de la fecha es inválido. Utiliza 'dd/MM/yyyy'.");
        //}

        var action = await _calendarUnitOfWork.GetTotalRecordsAsync(pagination, queryDate);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }


}
