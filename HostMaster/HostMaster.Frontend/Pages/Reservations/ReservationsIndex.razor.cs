using HostMaster.Frontend.Repositories;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace HostMaster.Frontend.Pages.Reservations
{
    public partial class ReservationsIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;

        public List<Reservation>? Reservations { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Reservation>>("api/reservations");
            Reservations = responseHttp.Response;
        }
    }
}