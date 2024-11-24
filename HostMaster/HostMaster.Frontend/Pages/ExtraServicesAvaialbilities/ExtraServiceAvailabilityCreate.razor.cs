using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Pages.Countries;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace HostMaster.Frontend.Pages.ExtraServicesAvaialbilities;

public partial class ExtraServiceAvailabilityCreate
{
    private ExtraServiceAvailabilityForm? extraServiceAvailabilityForm;
    private ServiceAvailabilityDTO serviceAvailability = new();

    [Parameter] public int ServiceId { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        serviceAvailability.ServiceId = ServiceId;

        var responseHttp = await Repository.PostAsync("api/extraServices/availability", serviceAvailability);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync(Localizer["Error"], Localizer[message!]);
            return;
        }

        Return();
        var toast = SweetAlertService.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordCreatedOk"]);
    }

    private void Return()
    {
        extraServiceAvailabilityForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo($"/extraServices/{ServiceId}/availabilities");
    }
}