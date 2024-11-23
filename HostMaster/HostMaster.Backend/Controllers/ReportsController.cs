using HostMaster.Backend.UnitsOfWork.Implementations;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HostMaster.Backend.Controllers;

public class ReportsController : GenericController<OccupationDataDto>
{
    private readonly IReportsUnitOfWork _reportsUnitOfWork;

    public ReportsController(IGenericUnitOfWork<OccupationDataDto> unitOfWork, IReportsUnitOfWork reportsUnitOfWork) : base(unitOfWork)
    {
        _reportsUnitOfWork = reportsUnitOfWork;
    }

    [HttpGet("OccupationData")]
    public async Task<IActionResult> GetOccupancyPercentageByAccommodationAsync([FromQuery] int accommodationId, DateTime startDate, DateTime endDate)
    {

        var response = await _reportsUnitOfWork.GetOccupancyPercentageByAccommodationAsync( accommodationId,  startDate,  endDate);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

}
