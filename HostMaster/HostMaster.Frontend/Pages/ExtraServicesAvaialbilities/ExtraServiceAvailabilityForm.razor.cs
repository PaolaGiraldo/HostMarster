using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace HostMaster.Frontend.Pages.ExtraServicesAvaialbilities;

public partial class ExtraServiceAvailabilityForm
{
	private EditContext editContext = null!;

	public bool IsAvailable { get; set; } = false;

	protected override void OnInitialized()
	{
		editContext = new(ServiceAvailabilityDTO);
	}

	[EditorRequired, Parameter] public int ServiceId { get; set; }
	[EditorRequired, Parameter] public ServiceAvailabilityDTO ServiceAvailabilityDTO { get; set; } = new ServiceAvailabilityDTO();
	[EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
	[EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

	[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
	[Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

	public bool FormPostedSuccessfully { get; set; } = false;

	private async Task OnBeforeInternalNavigation(LocationChangingContext context)
	{
		var formWasEdited = editContext.IsModified();

		if (!formWasEdited || FormPostedSuccessfully)
		{
			return;
		}

		var result = await SweetAlertService.FireAsync(new SweetAlertOptions
		{
			Title = Localizer["Confirmation"],
			Text = Localizer["LeaveAndLoseChanges"],
			Icon = SweetAlertIcon.Warning,
			ShowCancelButton = true
		});

		var confirm = !string.IsNullOrEmpty(result.Value);
		if (confirm)
		{
			return;
		}

		context.PreventNavigation();
	}
}