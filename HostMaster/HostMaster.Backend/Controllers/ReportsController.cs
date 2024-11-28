using HostMaster.Backend.UnitsOfWork.Implementations;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

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
        var response = await _reportsUnitOfWork.GetOccupancyPercentageByAccommodationAsync(accommodationId, startDate, endDate);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpGet("occupancy/pdf")]
    public async Task<IActionResult> DownloadOccupancyReportPdf(int accommodationId, DateTime startDate, DateTime endDate)
    {
        // Validar parámetros
        if (accommodationId <= 0 || startDate > endDate)
        {
            return BadRequest("Los parámetros proporcionados no son válidos.");
        }

        try
        {
            // Obtener los datos del reporte
            var reportData = await _reportsUnitOfWork.GetOccupancyPercentageByAccommodationAsync(accommodationId, startDate, endDate);

            if (!reportData.WasSuccess || reportData.Result == null || !reportData.Result.Any())
            {
                return NotFound("No se encontraron datos para generar el reporte.");
            }

            // Generar el PDF
            var pdfBytes = GeneratePdf(reportData.Result);

            // Retornar el archivo PDF como respuesta
            return File(pdfBytes, "application/pdf", $"Reporte_Ocupacion_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
        catch (Exception ex)
        {
            // Registrar el error
            Console.WriteLine($"Error al generar el reporte PDF: {ex.Message}");
            return StatusCode(500, "Se produjo un error interno al generar el reporte.");
        }
    }

    private byte[] GeneratePdf(IEnumerable<OccupationDataDto> data)
    {
        if (data == null || !data.Any())
        {
            throw new ArgumentException("No se proporcionaron datos para generar el PDF.");
        }

        using var stream = new MemoryStream();
        var document = new iTextSharp.text.Document();
        PdfWriter.GetInstance(document, stream);

        document.Open();

        // Título del reporte
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
        var title = new Paragraph("Reporte de Ocupación", titleFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20f
        };
        document.Add(title);

        // Crear tabla
        var table = new PdfPTable(4)
        {
            WidthPercentage = 100,
            SpacingBefore = 10f,
            SpacingAfter = 10f
        };

        // Encabezados de la tabla
        var headers = new[] { "Fecha", "Habitaciones Ocupadas", "Total de Habitaciones", "% Ocupación" };
        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);

        foreach (var header in headers)
        {
            var cell = new PdfPCell(new Phrase(header, headerFont))
            {
                BackgroundColor = BaseColor.GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            };
            table.AddCell(cell);
        }

        // Agregar datos a la tabla
        var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

        foreach (var item in data)
        {
            table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("yyyy-MM-dd"), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.OccupiedRooms.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.TotalRooms.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.OccupiedPercentage.ToString("F2") + "%", cellFont)) { Padding = 5 });
        }

        document.Add(table);

        // Cerrar el documento
        document.Close();

        return stream.ToArray();
    }

    private PdfPCell CreateCell(string content, bool isHeader = false)
    {
        var font = isHeader
            ? FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.BaseColor.WHITE)
            : FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.BaseColor.BLACK);

        var cell = new PdfPCell(new Phrase(content, font))
        {
            BackgroundColor = isHeader ? iTextSharp.text.BaseColor.DARK_GRAY : iTextSharp.text.BaseColor.WHITE,
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 5
        };

        return cell;
    }

    [HttpGet("MonthlyOccupationData")]
    public async Task<IActionResult> GetMonthlyOccupancyPercentageAsync([FromQuery] int accommodationId, DateTime startDate, DateTime endDate)
    {
        var response = await _reportsUnitOfWork.GetMonthlyOccupancyPercentageAsync(accommodationId, startDate, endDate);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpGet("monthly-occupancy/pdf")]
    public async Task<IActionResult> DownloadMonthlyOccupancyReportPdf([FromQuery] int accommodationId, DateTime startDate, DateTime endDate)
    {
        // Validar parámetros
        if (accommodationId <= 0 || startDate > endDate)
        {
            return BadRequest("Los parámetros proporcionados no son válidos.");
        }

        try
        {
            // Obtener los datos del reporte
            var reportData = await _reportsUnitOfWork.GetMonthlyOccupancyPercentageAsync(accommodationId, startDate, endDate);

            if (!reportData.WasSuccess || reportData.Result == null || !reportData.Result.Any())
            {
                return NotFound("No se encontraron datos para generar el reporte.");
            }

            // Generar el PDF
            var pdfBytes = GenerateMonthlyPdf(reportData.Result);

            // Retornar el archivo PDF como respuesta
            return File(pdfBytes, "application/pdf", $"Reporte_Ocupacion_Mensual_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
        catch (Exception ex)
        {
            // Registrar el error
            Console.WriteLine($"Error al generar el reporte PDF: {ex.Message}");
            return StatusCode(500, "Se produjo un error interno al generar el reporte.");
        }
    }

    private byte[] GenerateMonthlyPdf(IEnumerable<MonthlyOccupancyDto> data)
    {
        if (data == null || !data.Any())
        {
            throw new ArgumentException("No se proporcionaron datos para generar el PDF.");
        }

        using var stream = new MemoryStream();
        var document = new iTextSharp.text.Document();
        PdfWriter.GetInstance(document, stream);

        document.Open();

        // Título del reporte
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
        var title = new Paragraph("Reporte de Ocupación Mensual", titleFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20f
        };
        document.Add(title);

        // Crear tabla
        var table = new PdfPTable(5)
        {
            WidthPercentage = 100,
            SpacingBefore = 10f,
            SpacingAfter = 10f
        };

        // Encabezados de la tabla
        var headers = new[] { "Año", "Mes", "Habitaciones Ocupadas", "Total de Habitaciones", "% Ocupación" };
        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);

        foreach (var header in headers)
        {
            var cell = new PdfPCell(new Phrase(header, headerFont))
            {
                BackgroundColor = BaseColor.GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            };
            table.AddCell(cell);
        }

        // Agregar datos a la tabla
        var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

        foreach (var item in data)
        {
            table.AddCell(new PdfPCell(new Phrase(item.Year.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.Month.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.OccupiedRooms.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.TotalRooms.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.OccupiedPercentage.ToString("F2") + "%", cellFont)) { Padding = 5 });
        }

        document.Add(table);

        // Cerrar el documento
        document.Close();

        return stream.ToArray();
    }

    [HttpGet("MonthlyRevenue")]
    public async Task<IActionResult> GetMonthlyRevenueAsync([FromQuery] int accommodationId, DateTime startDate, DateTime endDate)
    {
        if (accommodationId <= 0 || startDate > endDate)
        {
            return BadRequest("Los parámetros proporcionados no son válidos.");
        }

        var response = await _reportsUnitOfWork.GetMonthlyRevenueAsync(accommodationId, startDate, endDate);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return NotFound(response.Message);
    }

    [HttpGet("MonthlyRevenue/pdf")]
    public async Task<IActionResult> DownloadMonthlyRevenueReportPdf([FromQuery] int accommodationId, DateTime startDate, DateTime endDate)
    {
        if (accommodationId <= 0 || startDate > endDate)
        {
            return BadRequest("Los parámetros proporcionados no son válidos.");
        }

        try
        {
            var response = await _reportsUnitOfWork.GetMonthlyRevenueAsync(accommodationId, startDate, endDate);

            if (!response.WasSuccess || response.Result == null || !response.Result.Any())
            {
                return NotFound("No se encontraron datos para generar el reporte.");
            }

            // Generar el PDF
            var pdfBytes = GenerateMonthlyRevenuePdf(response.Result);

            return File(pdfBytes, "application/pdf", $"Ingresos_Mensuales_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar el reporte PDF: {ex.Message}");
            return StatusCode(500, "Se produjo un error interno al generar el reporte.");
        }
    }

    private byte[] GenerateMonthlyRevenuePdf(IEnumerable<MonthlyRevenueDto> data)
    {
        if (data == null || !data.Any())
        {
            throw new ArgumentException("No se proporcionaron datos para generar el PDF.");
        }

        using var stream = new MemoryStream();
        var document = new iTextSharp.text.Document();
        PdfWriter.GetInstance(document, stream);

        document.Open();

        // Título del reporte
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
        var title = new Paragraph("Reporte de Ingresos Mensuales", titleFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20f
        };
        document.Add(title);

        // Crear tabla
        var table = new PdfPTable(3)
        {
            WidthPercentage = 100,
            SpacingBefore = 10f,
            SpacingAfter = 10f
        };

        // Encabezados de la tabla
        var headers = new[] { "Año", "Mes", "Ingresos Totales" };
        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);

        foreach (var header in headers)
        {
            var cell = new PdfPCell(new Phrase(header, headerFont))
            {
                BackgroundColor = BaseColor.GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            };
            table.AddCell(cell);
        }

        // Agregar datos a la tabla
        var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

        foreach (var item in data)
        {
            table.AddCell(new PdfPCell(new Phrase(item.Year.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.Month.ToString(), cellFont)) { Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(item.TotalRevenue.ToString("C"), cellFont)) { Padding = 5 });
        }

        document.Add(table);

        // Cerrar el documento
        document.Close();

        return stream.ToArray();
    }
}