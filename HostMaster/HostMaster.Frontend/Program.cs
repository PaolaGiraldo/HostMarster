using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend;
using HostMaster.Frontend.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7242") });

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddLocalization();
builder.Services.AddSweetAlert2();
builder.Services.AddMudServices();
builder.Services.AddBlazoredModal();

await builder.Build().RunAsync();