using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace HostMaster.Frontend.Pages.Quotations;

public partial class QuotationCreate
{
    private QuotationForm? quotationForm;
    private QuotationDTO quotationDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        Console.WriteLine("HOLLLJJHGJ");
        var responseHttp = await Repository.PostAsync("/api/Quotations/request-quote", quotationDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        ClearForm();
        Snackbar.Add(Localizer["RecordCreatedOk"], Severity.Success);
    }

    private void Return()
    {
        quotationForm!.FormPostedSuccessfully = true;
        //NavigationManager.NavigateTo("/maintenances");
    }

    private void ClearForm()
    {
        // Limpiar el modelo y restablecer el estado del formulario
        quotationDTO = new QuotationDTO(); // Restablecer el modelo a su estado inicial
        StateHasChanged(); // Actualizar la interfaz de usuario
    }
}