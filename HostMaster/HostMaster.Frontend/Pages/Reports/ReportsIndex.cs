namespace HostMaster.Frontend.Pages.Reports;

using HostMaster.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public partial class ReportsIndex
{
    private MudDateRangePicker _dateRangePicker;
    private string AccommodationId { get; set; } = string.Empty;

    private bool IsDownloadDisabled =>
       string.IsNullOrWhiteSpace(AccommodationId) ||
       DateRange?.Start == null ||
       DateRange?.End == null;

    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public HttpClient HttpClient { get; set; }

    // Datos para la gráfica
    private List<string> ChartLabels { get; set; } = new List<string>();

    private List<ChartSeries> ChartData { get; set; } = new List<ChartSeries>();

    // Rango de fechas
    private DateRange _dateRange;

    private DateRange DateRange
    {
        get => _dateRange;
        set
        {
            _dateRange = value;
            _ = FetchOccupancyData(); // Llama a FetchOccupancyData al cambiar el rango de fechas
            _ = FetchMonthlyRevenueData(); // Llama a FetchOccupancyData al cambiar el rango de fechas
        }
    }

    private int Index = -1; // Default value cannot be 0 -> first selected index is 0.
    public ChartOptions Options { get; set; } = new ChartOptions();

    public List<ChartSeries> Series { get; set; } = new List<ChartSeries>();
    public List<ChartSeries> Seriesc { get; set; } = new List<ChartSeries>();

    public string[] XAxisLabels { get; set; } = Array.Empty<string>();
    public string[] XAxisLabelsc { get; set; } = Array.Empty<string>();

    private int Indexc = 1; // Valor predeterminado, no puede ser 0 ya que el primer SelectedIndex es 0.

    protected override async Task OnInitializedAsync()
    {
        await FetchOccupancyData();
        await FetchMonthlyRevenueData();
    }

    private async Task FetchOccupancyData()
    {
        //if (IsDownloadDisabled)
        //{
        //     Snackbar.Add("Por favor, complete los campos necesarios.", Severity.Error);
        //     return;
        // }

        if (string.IsNullOrWhiteSpace(AccommodationId))
        {
            AccommodationId = "1";
        }

        try
        {
            // Establecer valores predeterminados si DateRange es null o si sus propiedades Start/End son null
            var startDate = (DateRange?.Start ?? DateTime.Today.AddYears(-1)).ToString("yyyy-MM-dd");
            var endDate = (DateRange?.End ?? DateTime.Today).ToString("yyyy-MM-dd");

            // var startDate = DateRange.Start.Value.ToString("yyyy-MM-dd");
            // var endDate = DateRange.End.Value.ToString("yyyy-MM-dd");
            var url = $"OccupationData?accommodationId={AccommodationId}&startDate={startDate}&endDate={endDate}";

            var response = await HttpClient.GetFromJsonAsync<List<OccupationDataDto>>(url);

            if (response != null && response.Any())
            {
                ChartLabels.Clear();
                var occupancyPercentages = new List<double>();

                // Convertir los valores de OccupiedPercentage a un arreglo de double[]
                var percentages = response.Select(item => item.OccupiedPercentage).ToArray();

                XAxisLabels = percentages.Select(value => value.ToString("F2")).ToArray();

                // Crear la serie con los datos convertidos
                Series = new List<ChartSeries> {
                    new ChartSeries() {
                        Name = "Ocupación (%)",
                        Data = percentages
                    }};
            }
            else
            {
                Snackbar.Add("No se encontraron datos para el rango de fechas seleccionado.", Severity.Warning);
                ChartLabels.Clear();
                ChartData.Clear();
            }

            StateHasChanged(); // Asegura que la UI se actualice
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error al cargar los datos de ocupación: {ex.Message}", Severity.Error);
        }
    }

    private async Task FetchMonthlyRevenueData()
    {
        if (string.IsNullOrWhiteSpace(AccommodationId))
        {
            AccommodationId = "1";
        }

        try
        {
            var startDate = (DateRange?.Start ?? DateTime.Today.AddYears(-1)).ToString("yyyy-MM-dd");
            var endDate = (DateRange?.End ?? DateTime.Today).ToString("yyyy-MM-dd");
   
            var url = $"MonthlyRevenue?accommodationId={AccommodationId}&startDate={startDate}&endDate={endDate}";

            var response = await HttpClient.GetFromJsonAsync<List<MonthlyRevenueDto>>(url);

            if (response != null && response.Any())
            {
                ChartLabels.Clear();
                var occupancyPercentages = new List<double>();

                var Revenues = response.Select(item => (double)item.TotalRevenue).ToArray();

                var RevenueDates = response.Select(item => $"{item.Year}-{item.Month:D2}").ToArray();

                XAxisLabelsc = RevenueDates;

                // Crear la serie con los datos convertidos
                Seriesc = new List<ChartSeries> {
                    new ChartSeries() {
                        Name = "Revenue",
                        Data = Revenues
                    }};
            }
            else
            {
                Snackbar.Add("No se encontraron datos para el rango de fechas seleccionado.", Severity.Warning);
                ChartLabels.Clear();
                ChartData.Clear();
            }

            StateHasChanged(); // Asegura que la UI se actualice
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error al cargar los datos de ocupación: {ex.Message}", Severity.Error);
        }
    }

    private async Task DownloadReport()
    {
        if (string.IsNullOrWhiteSpace(AccommodationId))
        {
            Snackbar.Add("Por favor, ingrese un Accommodation ID válido.", Severity.Error);
            return;
        }

        if (DateRange.Start == null || DateRange.End == null)
        {
            Snackbar.Add("Por favor, seleccione un rango de fechas válido.", Severity.Error);
            return;
        }

        try
        {
            var startDate = DateRange.Start.Value.ToString("yyyy-MM-dd");
            var endDate = DateRange.End.Value.ToString("yyyy-MM-dd");
            var url = $"occupancy/pdf?accommodationId={AccommodationId}&startDate={startDate}&endDate={endDate}";

            var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var fileName = $"reporte_{AccommodationId}_{startDate}_al_{endDate}.pdf";
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