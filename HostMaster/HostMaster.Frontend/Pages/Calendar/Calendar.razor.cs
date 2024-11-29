using HostMaster.Frontend.Services;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;

namespace HostMaster.Frontend.Pages.Calendar;

public class CalendarBase : ComponentBase
{
    [Inject]
    protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected DateSelectionService DateSelectionService { get; set; }

    [Inject] public IStringLocalizer<Literals> Localizer { get; set; } = null!;

    // add if to load currentMonth when go back page.
    //################################TO DO #######################
    // if service date null then calculate currentMonth otherwise load from service
    protected DateTime CurrentMonth { get; set; } = DateTime.Now;

    protected List<List<DateTime?>> WeeksInMonth { get; set; }

    protected override void OnInitialized()
    {
        GenerateCalendar(CurrentMonth);
    }

    // Método para generar el calendario para el mes actual
    protected void GenerateCalendar(DateTime month)
    {
        // Obtener el primer y último día del mes
        var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
        var lastDayOfMonth = new DateTime(month.Year, month.Month, daysInMonth);

        // Crear una lista para almacenar las semanas del mes
        WeeksInMonth = new List<List<DateTime?>>();

        // Inicializar el contador de días
        var currentDay = firstDayOfMonth;

        // Obtener el día de la semana del primer día del mes (Lunes = 1, Domingo = 7)
        int startDayOfWeek = ((int)firstDayOfMonth.DayOfWeek == 0) ? 7 : (int)firstDayOfMonth.DayOfWeek;

        // Crear la primera semana
        var currentWeek = new List<DateTime?>();

        // Añadir días vacíos antes del primer día del mes (si el mes no empieza un lunes)
        for (int i = 1; i < startDayOfWeek; i++)
        {
            currentWeek.Add(null);
        }

        // Añadir los días del mes a las semanas correspondientes
        while (currentDay <= lastDayOfMonth)
        {
            if (currentWeek.Count == 7)
            {
                WeeksInMonth.Add(currentWeek);
                currentWeek = new List<DateTime?>();
            }

            currentWeek.Add(currentDay);
            currentDay = currentDay.AddDays(1);
        }

        // Añadir los días vacíos al final de la última semana si es necesario
        while (currentWeek.Count < 7)
        {
            currentWeek.Add(null);
        }

        // Añadir la última semana a la lista
        WeeksInMonth.Add(currentWeek);
    }

    protected void PreviousMonth()
    {
        CurrentMonth = CurrentMonth.AddMonths(-1);
        GenerateCalendar(CurrentMonth);
    }

    protected void NextMonth()
    {
        CurrentMonth = CurrentMonth.AddMonths(1);
        GenerateCalendar(CurrentMonth);
    }

    // protected void OnDateSelected(DateTime date)
    // {
    //     // Guardamos la fecha seleccionada en el servicio
    //    DateSelectionService.SelectedDate = date;

    // Navegamos a la página de reservas sin pasar parámetros en la URL
    //  NavigationManager.NavigateTo("/CalendarList");
    // }

    protected async void OnDateSelected(DateTime date)
    {
        // Guardamos la fecha seleccionada en el servicio
        DateSelectionService.SelectedDate = date;

        // Obtener el estado de autenticación
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        // Comprobar si el usuario está autenticado y obtener su rol
        if (user.Identity?.IsAuthenticated == true)
        {
            if (user.IsInRole("Admin"))
            {
                NavigationManager.NavigateTo("/CalendarList");
            }
            else
            {
                // Si no tiene un rol conocido, redirigir a una página por defecto
                NavigationManager.NavigateTo("/AvailableRooms");
            }
        }
        else
        {
            // Redirigir a la página de inicio de sesión si no está autenticado
            NavigationManager.NavigateTo("/Login");
        }
    }

    protected bool IsToday(DateTime date)
    {
        return date.Date == DateTime.Now.Date;
    }
}