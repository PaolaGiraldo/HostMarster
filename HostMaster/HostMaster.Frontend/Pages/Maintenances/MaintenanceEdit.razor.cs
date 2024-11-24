using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace HostMaster.Frontend.Pages.Maintenances;

public partial class MaintenanceEdit
{
    private MaintenanceForm? maintenanceForm;
    private MaintenanceDTO maintenanceDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<Maintenance>($"api/maintenances/{Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("maintenances");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            var maintenance = responseHttp.Response;
            maintenanceDTO = new MaintenanceDTO()
            {
                Id = maintenance!.Id,
                StartDate = maintenance!.StartDate,
                EndDate = maintenance!.EndDate,
                RoomId = maintenance!.RoomId,
                AccommodationId = maintenance!.AccommodationId,
                Observations = maintenance!.Observations
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/maintenances/full", maintenanceDTO);
        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(messageError!, Severity.Error);
            return;
        }
        Return();
        Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);
    }

    private void Return()
    {
        maintenanceForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/maintenances");
    }
}