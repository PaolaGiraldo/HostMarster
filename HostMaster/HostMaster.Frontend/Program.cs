using CurrieTechnologies.Razor.SweetAlert2;
using HostMaster.Frontend;
using HostMaster.Frontend.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7091") });

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();