using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using MudBlazor;
using static System.Net.WebRequestMethods;

namespace HostMaster.Frontend.Pages.ContactForm;

public partial class ContactForm
{
    private MudForm form;
    private string firstName { get; set; }
    private string lastName { get; set; }
    private string email { get; set; }
    private string phone { get; set; }
    private string message { get; set; }

    private bool loading;
    private ContactFormDTO contactFormDTO = new();

    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private async Task HandleSubmit()
    {
        if (ValidateForm())
        {
            var responseHttp = await Repository.PostAsync<ContactFormDTO>("/api/accounts/ContactForm", contactFormDTO);
            loading = true;
            NavigationManager.NavigateTo("/");
            await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = Localizer["Confirmation"],
                Text = "Muchas gracias por contactarnos, reponderemos lo mas pronto posible",
                Icon = SweetAlertIcon.Info,
            });
        }
    }

    private bool ValidateForm()
    {
        var hasErrors = false;
        if (string.IsNullOrEmpty(contactFormDTO.FirstName))
        {
            Snackbar.Add(string.Format(Localizer["RequiredField"], string.Format(Localizer["FirstName"])), Severity.Error);
            hasErrors = true;
        }
        if (string.IsNullOrEmpty(contactFormDTO.LastName))
        {
            Snackbar.Add(string.Format(Localizer["RequiredField"], string.Format(Localizer["LastName"])), Severity.Error);
            hasErrors = true;
        }
        if (string.IsNullOrEmpty(contactFormDTO.Phone))
        {
            Snackbar.Add(string.Format(Localizer["RequiredField"], string.Format(Localizer["PhoneNumber"])), Severity.Error);
            hasErrors = true;
        }
        if (string.IsNullOrEmpty(contactFormDTO.Email))
        {
            Snackbar.Add(string.Format(Localizer["RequiredField"], string.Format(Localizer["Email"])), Severity.Error);
            hasErrors = true;
        }
        if (string.IsNullOrEmpty(contactFormDTO.Message))
        {
            Snackbar.Add(string.Format(Localizer["RequiredField"], string.Format(Localizer["Message"])), Severity.Error);
            hasErrors = true;
        }
        return !hasErrors;
    }
}