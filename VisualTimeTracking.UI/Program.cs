global using VisualTimeTracking.UI.Services;
global using VisualTimeTracking.Common.ResponseModel;
global using VisualTimeTracking.Common.ViewModels;
using VisualTimeTracking.UI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Toast;
using MudBlazor.Services;
using VisualTimeTracking.UI.ApiServices;
using Microsoft.AspNetCore.Components.Authorization;
using VisualTimeTracking.UI.Helper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredToast();
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<ILocalStorageServices, LocalStorageService>();
builder.Services.AddScoped<TimeTrackingApi, TimeTrackingApi>(); 
builder.Services.AddScoped<Formater>();
 
await builder.Build().RunAsync();
