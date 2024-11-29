namespace HostMaster.Frontend.Pages.Reports;

using HostMaster.Shared.DTOs;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
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
    [Inject] public IStringLocalizer<Literals> Localizer { get; set; } = null!;

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
        if (string.IsNullOrWhiteSpace(AccommodationId))
        {
            AccommodationId = "1";
        }

        try
        {
            var startDate = (DateRange?.Start ?? DateTime.Today.AddYears(-1)).ToString("yyyy-MM-dd");
            var endDate = (DateRange?.End ?? DateTime.Today).ToString("yyyy-MM-dd");

            var url = $"OccupationData?accommodationId={AccommodationId}&startDate={startDate}&endDate={endDate}";

            var response = await HttpClient.GetFromJsonAsync<List<OccupationDataDto>>(url);

            if (response != null && response.Any())
            {
                ChartLabels.Clear();
                var percentages = response.Select(item => item.OccupiedPercentage).ToArray();

                XAxisLabels = percentages.Select(value => "").ToArray();

                Series = new List<ChartSeries> {
                    new ChartSeries() {
                        Name = Localizer["OccupancyPercentage"],
                        Data = percentages
                    }};
            }
            else
            {
                Snackbar.Add(Localizer["NoDataForSelectedRange"], Severity.Warning);
                ChartLabels.Clear();
                ChartData.Clear();
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add(Localizer["ErrorLoadingOccupancyData", ex.Message], Severity.Error);
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
                var revenues = response.Select(item => (double)item.TotalRevenue).ToArray();
                var revenueDates = response.Select(item => $"{item.Year}-{item.Month:D2}").ToArray();

                XAxisLabelsc = revenueDates;

                Seriesc = new List<ChartSeries> {
                    new ChartSeries() {
                        Name = Localizer["Revenue"],
                        Data = revenues
                    }};
            }
            else
            {
                Snackbar.Add(Localizer["NoDataForSelectedRange"], Severity.Warning);
                ChartLabels.Clear();
                ChartData.Clear();
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add(Localizer["ErrorLoadingRevenueData", ex.Message], Severity.Error);
        }
    }

    private async Task DownloadReport()
    {
        if (string.IsNullOrWhiteSpace(AccommodationId))
        {
            Snackbar.Add(Localizer["EnterValidAccommodationId"], Severity.Error);
            return;
        }

        if (DateRange.Start == null || DateRange.End == null)
        {
            Snackbar.Add(Localizer["SelectValidDateRange"], Severity.Error);
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

                Snackbar.Add(Localizer["ReportDownloaded"], Severity.Success);
            }
            else
            {
                Snackbar.Add(Localizer["ErrorGeneratingReport"], Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(Localizer["UnexpectedError", ex.Message], Severity.Error);
        }
    }

    private async Task DownloadRevenueReport()
    {
        if (string.IsNullOrWhiteSpace(AccommodationId))
        {
            Snackbar.Add(Localizer["EnterValidAccommodationId"], Severity.Error);
            return;
        }

        if (DateRange.Start == null || DateRange.End == null)
        {
            Snackbar.Add(Localizer["SelectValidDateRange"], Severity.Error);
            return;
        }

        try
        {
            var startDate = DateRange.Start.Value.ToString("yyyy-MM-dd");
            var endDate = DateRange.End.Value.ToString("yyyy-MM-dd");
            var url = $"MonthlyRevenue/pdf?accommodationId={AccommodationId}&startDate={startDate}&endDate={endDate}";

            var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var fileName = $"reporte_ingresos_{AccommodationId}_{startDate}_al_{endDate}.pdf";
                var content = await response.Content.ReadAsByteArrayAsync();
                await JSRuntime.InvokeVoidAsync("downloadFile", fileName, "application/pdf", content);

                Snackbar.Add(Localizer["RevenueReportDownloaded"], Severity.Success);
            }
            else
            {
                Snackbar.Add(Localizer["ErrorGeneratingRevenueReport"], Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(Localizer["UnexpectedError", ex.Message], Severity.Error);
        }
    }
}