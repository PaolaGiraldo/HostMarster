using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace HostMaster.Frontend.Pages.ExtraServices;

public partial class ExtraServicesEdit
{
    private ExtraServiceDTO? extraServiceDTO;
    private ExtraServicesForm? extraServicesForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<ExtraService>($"api/extraServices/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("extraServices");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            var extraServiceJson = await responseHttp.HttpResponseMessage.Content.ReadAsStringAsync();
            var extraServiceType = responseHttp.Response;

            extraServiceDTO = new ExtraServiceDTO()
            {
                Id = extraServiceDTO!.Id,
                ServiceName = extraServiceDTO.ServiceName,
                ServiceDescription = extraServiceDTO.ServiceDescription,
                Price = extraServiceDTO.Price,
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/extraServices", extraServiceDTO);

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
        extraServicesForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("extraServices");
    }
}