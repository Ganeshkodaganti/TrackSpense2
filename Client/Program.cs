using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrackSpense.Client;

//adding mud blazor package
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TrackSpense.Client.Providers;
using TrackSpense.Client.StateContainers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//addign mudblazer to the builder 
builder.Services.AddMudServices();

//adding local storage
builder.Services.AddBlazoredLocalStorage();

//registering our 'CustomAuthProvider' in 'Program.cs'
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
/*builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7045/") });*/

builder.Services.AddSingleton<SummaryStateContainer>();
builder.Services.AddSingleton<CategoryStateContainer>();
builder.Services.AddSingleton<TransactionStateContainer>();

await builder.Build().RunAsync();
