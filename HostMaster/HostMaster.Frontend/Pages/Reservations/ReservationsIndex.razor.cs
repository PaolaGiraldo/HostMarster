using HostMaster.Frontend.Repositories;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace HostMaster.Frontend.Pages.Reservations
{
    public partial class ReservationsIndex
    {
        [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

        [Inject] private IRepository Repository { get; set; } = null!;

        public List<Reservation>? Reservations { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Reservation>>("api/reservations");
            Reservations = responseHttp.Response;
        }
    }
}