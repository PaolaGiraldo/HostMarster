using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;

namespace HostMaster.Frontend.Pages.ExtraServices;

public partial class ExtraServicesForm
{
	private List<IBrowserFile> files = new List<IBrowserFile>();

	private EditContext editContext = null!;

	protected override void OnInitialized()
	{
		editContext = new(ExtraServiceDTO);
	}

	private ExtraService selectedExtraService = new();

	[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
	[Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
	[Inject] private IRepository Repository { get; set; } = null!;

	[EditorRequired, Parameter] public ExtraServiceDTO ExtraServiceDTO { get; set; } = null!;

	[EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
	[EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

	public bool FormPostedSuccessfully { get; set; } = false;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
	}

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
			ShowCancelButton = true,
			CancelButtonText = Localizer["Cancel"],
		});

		var confirm = !string.IsNullOrEmpty(result.Value);
		if (confirm)
		{
			return;
		}

		context.PreventNavigation();
	}
}