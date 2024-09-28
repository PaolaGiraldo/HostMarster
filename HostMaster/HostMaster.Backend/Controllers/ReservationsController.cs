using HostMaster.Backend.UnitsOfWork.Interfaces;
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
}