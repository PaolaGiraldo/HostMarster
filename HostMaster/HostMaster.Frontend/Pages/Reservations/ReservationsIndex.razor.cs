using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics.Metrics;

namespace HostMaster.Frontend.Pages.Reservations
{
    public partial class ReservationsIndex
    {
        [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        public List<Reservation>? Reservations { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHppt = await Repository.GetAsync<List<Reservation>>("api/reservations");
            if (responseHppt.Error)
            {
                var message = await responseHppt.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(Localizer["Error"], message, SweetAlertIcon.Error);
                return;
            }
            Reservations = responseHppt.Response!;
        }

        private async Task DeleteAsync(Reservation reservation)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = Localizer["Confirmation"],
                Text = string.Format(Localizer["DeleteConfirm"], Localizer["Reservation"], reservation.Room),
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = Localizer["Cancel"]
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync($"api/reservations/{reservation.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    var mensajeError = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(Localizer["Error"], mensajeError, SweetAlertIcon.Error);
                }
                return;
            }

            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
                ConfirmButtonText = Localizer["Yes"]
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordDeletedOk"]);
        }
    }
}