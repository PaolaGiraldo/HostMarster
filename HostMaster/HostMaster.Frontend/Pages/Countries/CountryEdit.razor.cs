﻿using System.Net;
using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.Entities;

namespace HostMaster.Frontend.Pages.Countries;

public partial class CountryEdit
{
    private Country? country;
    private FormWithName<Country>? countryForm;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [EditorRequired, Parameter] public int Id { get; set; }
    [CascadingParameter] private BlazoredModalInstance BlazoredModal { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var responseHttp = await Repository.GetAsync<Country>($"/api/countries/{Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/countries");
            }
            else
            {
                var messsage = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", messsage, SweetAlertIcon.Error);
            }
        }
        else
        {
            country = responseHttp.Response;
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("/api/countries", country);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message);
            return;
        }

        await BlazoredModal.CloseAsync(ModalResult.Ok());
        Return();

        var toast = SweetAlertService.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con éxito.");
    }

    private void Return()
    {
        countryForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/countries");
    }
}