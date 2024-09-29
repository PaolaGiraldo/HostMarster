using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace HostMaster.Frontend.Pages.Reservations;

public partial class ReservationEdit
{
    private ReservationForm? reservationForm;
    private ReservationDTO reservationDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
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
                await SweetAlertService.FireAsync(Localizer["Error"], messageError, SweetAlertIcon.Error);
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
        Console.WriteLine(responseHttp);
        if (responseHttp.Error)
        {
            var mensajeError = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync(Localizer["Error"], Localizer[mensajeError!], SweetAlertIcon.Error);
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
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordSavedOk"]);
    }

    private void Return()
    {
        reservationForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/reservations");
    }
}