using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;

namespace HostMaster.Frontend.Pages.Countries;

public partial class CountriesIndex
{
    private int currentPage = 1;
    private int totalPages;

    private bool loading = true;

    private MudTable<Country> table = new();

    private const string baseUrl = "api/countries";

    private int countryId = 1;

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

    public List<Country>? Countries { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadTotalRecordsAsync();
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

            dialog = DialogService.Show<CountryEdit>($"{Localizer["Edit"]} {Localizer["Country"]}", parameters, options);
        }
        else
        {
            dialog = DialogService.Show<CountryCreate>($"{Localizer["New"]} {Localizer["Country"]}", options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();
            await table.ReloadServerData();
        }
    }

    //private async Task FilterCallBack(string filter)
    //{
    //Filter = filter;
    //await ApplyFilterAsync();
    //StateHasChanged();
    //}

    //private async Task SelectedPageAsync(int page)
    //{
    //currentPage = page;
    //await LoadAsync(page);
    //}

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

    private void ValidateRecordsNumber()
    {
        if (RecordsNumber == 0)
        {
            RecordsNumber = 2;
        }
    }

    private async Task<TableData<Country>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;

        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<Country>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return new TableData<Country> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<Country> { Items = [], TotalItems = 0 };
        }
        return new TableData<Country>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task LoadPagesAsync()
    {
        var url = $"api/countries/totalPages?recordsnumber={RecordsNumber}";
        if (!string.IsNullOrEmpty(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<int>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }
        totalPages = responseHttp.Response;
    }

    //private async Task ApplyFilterAsync()
    //{
    //int page = 1;
    //await LoadAsync(page);
    //await SelectedPageAsync(page);
    //}

    private async Task DeleteAsync(Country country)
    {
        {
            var parameters = new DialogParameters
            {
                { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["Country"], country.Name) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };
            var dialog = DialogService.Show<ConfirmDialog>(Localizer["Confirmation"], parameters, options);
            var result = await dialog.Result;
            if (result!.Canceled)
            {
                return;
            }

            var responseHttp3 = await Repository.DeleteAsync($"api/roomphotos/by-roomId/{country.Id}");
            if (responseHttp3.Error)
            {
                var mensajeError = await responseHttp3.GetErrorMessageAsync();
                Snackbar.Add(Localizer[mensajeError!], Severity.Error);
                return;
            }

            var responseHttp = await Repository.DeleteAsync($"{baseUrl}/{country.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/countries");
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

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
    }
}