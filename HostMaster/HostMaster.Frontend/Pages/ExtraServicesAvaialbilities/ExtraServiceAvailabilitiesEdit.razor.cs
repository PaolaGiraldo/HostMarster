using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Pages.ExtraServices;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Security.Cryptography.X509Certificates;

namespace HostMaster.Frontend.Pages.ExtraServicesAvaialbilities;

public partial class ExtraServiceAvailabilitiesEdit
{
    private ServiceAvailabilityDTO? ServiceAvailabilityDTO;
    private ExtraServiceAvailabilityForm editExtraServiceAvailabilityForm;

    [Parameter] public int ServiceId { get; set; }
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<ServiceAvailability>($"api/extraServices/{ServiceId}/availabilities/");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo($"api/extraServices/{ServiceId}/availabilities/");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            ServiceAvailabilityDTO = new ServiceAvailabilityDTO
            {
                ServiceId = ServiceAvailabilityDTO!.ServiceId,
                StartDate = ServiceAvailabilityDTO.StartDate,
                EndDate = ServiceAvailabilityDTO.EndDate,
                IsAvailable = ServiceAvailabilityDTO.IsAvailable,
            };
        }
    }

    public async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/extraServices/availability", ServiceAvailabilityDTO);

        if (responseHttp.Error)
        {
            var mensajeError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer["Error"], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);
    }

    private void Return()
    {
        editExtraServiceAvailabilityForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo($"api/extraServices/{ServiceId}/availabilities/");
    }
}