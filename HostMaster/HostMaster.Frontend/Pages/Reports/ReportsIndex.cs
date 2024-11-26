namespace HostMaster.Frontend.Pages.Reports;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public partial class ReportsIndex
{
    private MudDateRangePicker _dateRangePicker;
    private DateRange DateRange { get; set; } = new DateRange();
    private bool IsDownloadDisabled => DateRange.Start == null || DateRange.End == null;

    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public HttpClient HttpClient { get; set; } // Renombrado de Http a HttpClient

    private string AccommodationId { get; set; } = string.Empty; // Campo para el Accommodation ID

    private async Task DownloadReport()
    {
        if (DateRange.Start == null || DateRange.End == null)
        {
            Snackbar.Add("Por favor, seleccione un rango de fechas válido.", Severity.Error);
            return;
        }

        try
        {
            var startDate = DateRange.Start.Value.ToString("yyyy-MM-dd");
            var endDate = DateRange.End.Value.ToString("yyyy-MM-dd");

            // Utiliza HttpClient en lugar de Http
            var response = await HttpClient.GetAsync($"occupancy/pdf?accommodationId={AccommodationId}&startDate={startDate}&endDate={endDate}");

            if (response.IsSuccessStatusCode)
            {
                var fileName = $"reporte_{startDate}_al_{endDate}.pdf";
                var content = await response.Content.ReadAsByteArrayAsync();
                await JSRuntime.InvokeVoidAsync("downloadFile", fileName, "application/pdf", content);

                Snackbar.Add("Reporte descargado exitosamente.", Severity.Success);
            }
            else
            {
                Snackbar.Add("Error al generar el reporte.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error inesperado: {ex.Message}", Severity.Error);
        }
    }
}
