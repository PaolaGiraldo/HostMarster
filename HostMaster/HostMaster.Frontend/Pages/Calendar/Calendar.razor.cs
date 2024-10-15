using HostMaster.Frontend.Services;
using Microsoft.AspNetCore.Components;

namespace HostMaster.Frontend.Pages.Calendar;

public class CalendarBase : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected DateSelectionService DateSelectionService { get; set; }

    // add if to load currentMonth when go back page.
    //################################TO DO #######################
    // if service date null then calculate currentMonth otherwise load from service 
    protected DateTime CurrentMonth { get; set; } = DateTime.Now;
    protected List<List<DateTime?>> WeeksInMonth { get; set; }

    protected override void OnInitialized()
    {
        GenerateCalendar(CurrentMonth);
    }

    // M�todo para generar el calendario para el mes actual
    protected void GenerateCalendar(DateTime month)
    {
        // Obtener el primer y �ltimo d�a del mes
        var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
        var lastDayOfMonth = new DateTime(month.Year, month.Month, daysInMonth);

        // Crear una lista para almacenar las semanas del mes
        WeeksInMonth = new List<List<DateTime?>>();

        // Inicializar el contador de d�as
        var currentDay = firstDayOfMonth;

        // Obtener el d�a de la semana del primer d�a del mes (Lunes = 1, Domingo = 7)
        int startDayOfWeek = ((int)firstDayOfMonth.DayOfWeek == 0) ? 7 : (int)firstDayOfMonth.DayOfWeek;

        // Crear la primera semana
        var currentWeek = new List<DateTime?>();

        // A�adir d�as vac�os antes del primer d�a del mes (si el mes no empieza un lunes)
        for (int i = 1; i < startDayOfWeek; i++)
        {
            currentWeek.Add(null);
        }

        // A�adir los d�as del mes a las semanas correspondientes
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

        // A�adir los d�as vac�os al final de la �ltima semana si es necesario
        while (currentWeek.Count < 7)
        {
            currentWeek.Add(null);
        }

        // A�adir la �ltima semana a la lista
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

    protected void OnDateSelected(DateTime date)
    {
        // Guardamos la fecha seleccionada en el servicio
        DateSelectionService.SelectedDate = date;

        // Navegamos a la p�gina de reservas sin pasar par�metros en la URL
        NavigationManager.NavigateTo("/CalendarList");
    }

    protected bool IsToday(DateTime date)
    {
        return date.Date == DateTime.Now.Date;
    }

}