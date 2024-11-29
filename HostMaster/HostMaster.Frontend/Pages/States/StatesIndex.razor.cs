using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Pages.Countries;
using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;
using static MudBlazor.Colors;

namespace HostMaster.Frontend.Pages.States;

public partial class StatesIndex
{
    private int currentPage = 1;
    private int totalPages;

    private bool loading = true;

    private MudTable<State> table = new();

    private const string baseUrl = "api/states";

    private int stateId = 1;

    private int totalRecords = 0;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 2;
    [CascadingParameter] private IModalService Modal { get; set; } = default!;

    public List<State>? States { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadTotalRecordsAsync();
    }

    private async Task LoadTotalRecordsAsync()
    {
        loading = true;
        var url = $"{baseUrl}/totalRecordsPaginated";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"?filter={Filter}";
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

    private async Task<TableData<State>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<State>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return new TableData<State> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<State> { Items = [], TotalItems = 0 };
        }
        return new TableData<State>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        IDialogReference? dialog;

        if (isEdit)
        {
            var parameters = new DialogParameters
                 {
                     { "Id", id }
                 };

            dialog = DialogService.Show<StateEdit>($"{Localizer["Edit"]} {Localizer["State"]}", parameters, options);
        }
        else
        {
            dialog = DialogService.Show<StateCreate>($"{Localizer["New"]} {Localizer["State"]}", options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();
            await table.ReloadServerData();
        }
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
    }

    private async Task OnPageChanged(int page)
    {
        currentPage = page;
        await table.ReloadServerData();
    }

    private async Task OnPageSizeChanged(int pageSize)
    {
        RecordsNumber = pageSize;
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
    }

    private async Task DeleteAsync(State state)
    {
        var parameters = new DialogParameters
            {
                { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["State"], state.Name) }
            };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };
        var dialog = DialogService.Show<ConfirmDialog>(Localizer["Confirmation"], parameters, options);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        var responseHttp = await Repository.DeleteAsync($"{baseUrl}/{state.Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/states");
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(Localizer[message!], Severity.Error);
            }
            return;
        }
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
        Snackbar.Add(Localizer["RecordDeletedOk"], Severity.Success);
    }
}