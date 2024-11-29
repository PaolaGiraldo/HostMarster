using HostMaster.Backend.Helpers;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MudBlazor.CategoryTypes;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("/api/[controller]")]
public class QuotationsController : ControllerBase
{
    private readonly IMailHelper _mailHelper;
    private readonly IConfiguration _configuration;

    public QuotationsController(IConfiguration configuration, IMailHelper mailHelper)
    {
        _configuration = configuration;
        _mailHelper = mailHelper;
    }

    [HttpPost("request-quote")]
    public async Task<ActionResponse<string>> SendQuoteAsync([FromBody] QuotationDTO quotationDTO)
    {
        Console.WriteLine(quotationDTO.Name);
        // Construir el cuerpo del correo
        var body = $"<h3>Solicitud de Cotización</h3>" +
                   $"<p><strong>Nombre:</strong> {quotationDTO.Name}</p>" +
                   $"<p><strong>Correo:</strong> {quotationDTO.Email}</p>" +
                   $"<p><strong>Teléfono:</strong> {quotationDTO.Phone}</p>" +
                   $"<p><strong>Fecha de Inicio:</strong> {quotationDTO.StartDate.ToString}</p>" +
                   $"<p><strong>Fecha de Fin:</strong> {quotationDTO.EndDate.ToString}</p>" +
                   $"<p><strong>Adultos:</strong> {quotationDTO.Adults}</p>" +
                   $"<p><strong>Niños:</strong> {quotationDTO.Children}</p>" +
                   $"<p><strong>Tipo de Habitación:</strong> {quotationDTO.RoomType}</p>" +
                   $"<p><strong>Observaciones:</strong> {quotationDTO.Observations}</p>";

        return _mailHelper.SendMail("Asesor Reservas", "hostmaster2024@gmail.com", "Nueva Solicitud de Cotizacion", body, "es");
    }
}