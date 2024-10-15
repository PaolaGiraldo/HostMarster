using HostMaster.Frontend.Pages.Rooms;
using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Services;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;

namespace HostMaster.Frontend.Pages.Calendar;

public partial class CalendarList
{

    private List<CalendarListDTO>? calendarList { get; set; }
    private MudTable<CalendarListDTO> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/calendar";
    private readonly string infoFormat = "{first_item}-{last_item} => {all_items}";

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

    [Inject] private DateSelectionService DateSelectionService { get; set; } = null!; 

    String? selectedDate;
    
    protected override async Task OnInitializedAsync()
    {

        selectedDate = DateSelectionService.SelectedDate.ToString("yyyy-MM-dd");
        await LoadTotalRecordsAsync();
    }

    private async Task LoadTotalRecordsAsync()
    {
        //selectedDate = DateSelectionService.SelectedDate.ToString("yyyy-MM-dd");

        loading = true;
        var url = $"{baseUrl}/totalRecordsPaginatedbydate?queryDate={selectedDate}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<int>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        totalRecords = responseHttp.Response;
        loading = false;
    }

    private async Task<TableData<CalendarListDTO>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {

        //selectedDate = DateSelectionService.SelectedDate.ToString("yyyy-MM-dd");

        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{baseUrl}/paginatedbydate/?&page={page}&recordsnumber={pageSize}&queryDate={selectedDate}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<CalendarListDTO>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return new TableData<CalendarListDTO> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<CalendarListDTO> { Items = [], TotalItems = 0 };
        }
        return new TableData<CalendarListDTO>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
    }

}