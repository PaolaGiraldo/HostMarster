using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Services;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace HostMaster.Frontend.Pages.Calendar;

public class AvailableRoomsBase : ComponentBase
{
    [Inject] protected IRepository Repository { get; set; } = null!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;
    [Inject] protected IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] protected DateSelectionService DateSelectionService { get; set; } = null!;

    protected List<Room>? availableRoomsList { get; set; }
    protected bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        // Obtener la fecha seleccionada del servicio
        var selectedDate = DateSelectionService.SelectedDate.ToString("yyyy-MM-dd");
        await LoadAvailableRoomsAsync(selectedDate);
    }

    protected async Task LoadAvailableRoomsAsync(string queryDate)
    {
        loading = true;

        // Construir la URL con el parámetro queryDate
        var url = $"api/Calendar/availableRooms?queryDate={queryDate}";

        // Realizar la solicitud al backend
        var responseHttp = await Repository.GetAsync<List<Room>>(url);
        if (!responseHttp.Error && responseHttp.Response != null)
        {
            availableRoomsList = responseHttp.Response;
        }
        else
        {
            availableRoomsList = new List<Room>();
        }

        loading = false;
    }

    protected void GoBack()
    {
        NavigationManager.NavigateTo("/Calendar");
    }
}