using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.Entities;
using HostMaster.Shared.DTOs;
using Microsoft.Extensions.Localization;
using HostMaster.Shared.Resources;
using MudBlazor;

namespace HostMaster.Frontend.Pages.States;

public partial class StateCreate
{
	private StateDTO stateDTO = new();
	private StateForm? stateForm;

	[Inject] private IRepository Repository { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;
	[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

	[Inject] private ISnackbar Snackbar { get; set; } = null!;

	[Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

	private async Task CreateAsync()
	{
		var responseHttp = await Repository.PostAsync("/api/states/full", stateDTO);
		if (responseHttp.Error)
		{
			var message = await responseHttp.GetErrorMessageAsync();
			Snackbar.Add(Localizer[message!], Severity.Error);
			return;
		}

		Return();
		Snackbar.Add(Localizer["RecordCreatedOk"], Severity.Success);
	}

	private void Return()
	{
		stateForm!.FormPostedSuccessfully = true;
		NavigationManager.NavigateTo($"/states");
	}
}