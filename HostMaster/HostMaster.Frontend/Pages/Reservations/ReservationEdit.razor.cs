using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace HostMaster.Frontend.Pages.Reservations;

public partial class ReservationEdit
{
    private ReservationForm? reservationForm;
    private ReservationDTO reservationDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<Reservation>($"api/reservations/{Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("reservations");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            var reservation = responseHttp.Response;
            reservationDTO = new ReservationDTO()
            {
                Id = reservation!.Id,
                StartDate = reservation!.StartDate,
                EndDate = reservation!.EndDate,
                RoomId = reservation!.RoomId,
                NumberOfGuests = reservation!.NumberOfGuests,
                CustomerDocument = reservation!.CustomerDocumentNumber,
                AccommodationId = reservation!.AccommodationId,
                ReservationState = reservation!.ReservationState,
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/reservations/full", reservationDTO);
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
        reservationForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/reservations");
    }
}