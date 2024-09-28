using System.Net;
using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.Entities;
using MudBlazor;
using HostMaster.Shared.Resources;
using Microsoft.Extensions.Localization;
using HostMaster.Frontend.Shared;

namespace HostMaster.Frontend.Pages.Countries;

public partial class CountriesIndex
{
    private int currentPage = 1;
    private int totalPages;
	private bool loading;
	private const string baseUrl = "api/countries";
	private MudTable<Country> table = new();
	private int totalRecords = 0;

	[Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

	[Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

	[Inject] private IDialogService DialogService { get; set; } = null!;

	[Inject] private ISnackbar Snackbar { get; set; } = null!;

	[Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;
    [CascadingParameter] private IModalService Modal { get; set; } = default!;

    public List<Country>? Countries { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
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

	private async Task SelectedRecordsNumberAsync(int recordsnumber)
    {
        RecordsNumber = recordsnumber;
        int page = 1;
        await LoadAsync(page);
        await SelectedPageAsync(page);
    }

    private async Task FilterCallBack(string filter)
    {
        Filter = filter;
        await ApplyFilterAsync();
        StateHasChanged();
    }

    private async Task SelectedPageAsync(int page)
    {
        currentPage = page;
        await LoadAsync(page);
    }

    private async Task LoadAsync(int page = 1)
    {
        if (!string.IsNullOrWhiteSpace(Page))
        {
            page = Convert.ToInt32(Page);
        }

        var ok = await LoadListAsync(page);
        if (ok)
        {
            await LoadPagesAsync();
        }
    }

    private void ValidateRecordsNumber()
    {
        if (RecordsNumber == 0)
        {
            RecordsNumber = 10;
        }
    }

    private async Task<bool> LoadListAsync(int page)
    {
        ValidateRecordsNumber();
        var url = $"api/countries?page={page}&recordsnumber={RecordsNumber}";
        if (!string.IsNullOrEmpty(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<Country>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return false;
        }
        Countries = responseHttp.Response;
        return true;
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

    private async Task ApplyFilterAsync()
    {
        int page = 1;
        await LoadAsync(page);
        await SelectedPageAsync(page);
    }

    private async Task DeleteAsycn(Country country)
    {
        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmación",
            Text = $"¿Estas seguro de querer borrar el país: {country.Name}?",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true,
        });
        var confirm = string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        var responseHttp = await Repository.DeleteAsync($"api/countries/{country.Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/countries");
            }
            else
            {
                var mensajeError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
            }
            return;
        }

        await LoadAsync();
        var toast = SweetAlertService.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con éxito.");
    }
}