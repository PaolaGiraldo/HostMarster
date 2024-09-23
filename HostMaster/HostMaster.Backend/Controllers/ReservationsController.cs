using HostMaster.Backend.Data;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReservationsController : GenericController<Reservation>
{
    public ReservationsController(IGenericUnitOfWork<Reservation> unitOfWork) : base(unitOfWork)
    {
    }
}