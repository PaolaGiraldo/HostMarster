using System.Net;
using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using HostMaster.Frontend.Repositories;
using HostMaster.Frontend.Shared;
using HostMaster.Shared.Entities;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Resources;
using Microsoft.Extensions.Localization;
using MudBlazor;
using static MudBlazor.Colors;

namespace HostMaster.Frontend.Pages.States;

public partial class StateEdit
{
	private StateDTO? stateDTO;
	private StateForm? stateForm;

	private Country selectedCountry = new();

	[Inject] private IRepository Repository { get; set; } = null!;

	[Inject] private NavigationManager NavigationManager { get; set; } = null!;
	[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
	[Inject] private ISnackbar Snackbar { get; set; } = null!;
	[Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

	[Parameter] public int Id { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var responseHttp = await Repository.GetAsync<State>($"api/states/{Id}");

		if (responseHttp.Error)
		{
			if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
			{
				NavigationManager.NavigateTo("states");
			}
			else
			{
				var messageError = await responseHttp.GetErrorMessageAsync();
				Snackbar.Add(messageError!, Severity.Error);
			}
		}
		else
		{
			var state = responseHttp.Response;
			stateDTO = new StateDTO()
			{
				Id = state!.Id,
				Name = state!.Name,
				CountryId = state.CountryId
			};
			selectedCountry = state.Country!;
		}
	}

	private async Task EditAsync()
	{
		var responseHttp = await Repository.PutAsync("api/states/full", stateDTO);

		if (responseHttp.Error)
		{
			var mensajeError = await responseHttp.GetErrorMessageAsync();
			Snackbar.Add(Localizer[mensajeError!], Severity.Error);
			return;
		}

		Return();
		Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);
	}

	private void Return()
	{
		stateForm!.FormPostedSuccessfully = true;
		NavigationManager.NavigateTo("states");
	}
}