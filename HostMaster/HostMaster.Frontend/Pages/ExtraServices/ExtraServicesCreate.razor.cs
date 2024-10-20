using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Pages.Countries;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace HostMaster.Frontend.Pages.ExtraServices;

public partial class ExtraServicesCreate
{
    private ExtraServicesForm? extraServiceForm;
    private ExtraServiceDTO extraServiceDTO = new();

    //private ExtraService extraService = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    //[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/extraServices", extraServiceDTO);
        //var responseHttp = await Repository.PostAsync("/api/extraServices", extraService);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            //await SweetAlertService.FireAsync(Localizer["Error"], message);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordCreatedOk"], Severity.Success);
        /*var toast = SweetAlertService.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordCreatedOk"]);*/
    }

    private void Return()
    {
        extraServiceForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/extraServices");
    }
}