using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Net.Http.Json;

namespace HostMaster.Frontend.Pages.Opinions
{
    public partial class OpinionVisualization
    {
        private List<Opinion> opinions = new List<Opinion>();
        private bool loading;
        private const string baseUrl = "api/opinions";

        [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadTotalRecordsAsync();
        }

        private async Task LoadTotalRecordsAsync()
        {
            loading = true;
            var url = $"{baseUrl}/best";

            var responseHttp = await Repository.GetAsync<List<Opinion>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(Localizer[message!], Severity.Error);
                return;
            }

            opinions = responseHttp.Response;
            loading = false;
        }
    }
}