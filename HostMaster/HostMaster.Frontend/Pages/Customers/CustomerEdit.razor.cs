using HostMaster.Frontend.Pages.Rooms;
using HostMaster.Frontend.Repositories;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Text.Json;

namespace HostMaster.Frontend.Pages.Customers;

public partial class CustomerEdit
{
    private CustomerDTO? customerDTO;

    private CustomerForm? customerForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<Customer>($"api/customers/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("customers");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            var roomJson = await responseHttp.HttpResponseMessage.Content.ReadAsStringAsync();
            var customer = responseHttp.Response;

            customerDTO = new CustomerDTO()
            {
                Id = customer!.Id,
                DocumentNumber = customer.DocumentNumber,
                DocumentType = customer.DocumentType,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/Customers", customerDTO);

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
        customerForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("customers");
    }
}