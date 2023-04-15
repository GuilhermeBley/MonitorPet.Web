using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MonitorPet.Ui.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<MonitorPet.Ui.Client.Services.LocalStorageService>();
builder.Services.AddScoped<MonitorPet.Ui.Client.Services.JwtTokenService>();
builder.Services.AddScoped<MonitorPet.Ui.Client.Services.UserInfoService>();

builder.Services.AddScoped<AuthenticationStateProvider, MonitorPet.Ui.Client.Services.AuthenticationStateService>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
