using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Http;
using SmallsOnline.Web.PublicSite.Client;
using SmallsOnline.Web.PublicSite.Client.Models;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<FavoritesOfStateContainer>();

builder.Services.AddScoped(
    sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);

builder.Services.AddHttpClient(
    name: "BaseAppClient",
    configureClient: (client) => { client.BaseAddress = new(builder.HostEnvironment.BaseAddress); }
);

string? apiUri = builder.Configuration.GetValue<string>("ApiUri");

if (apiUri is null)
{
    throw new InvalidOperationException("The API URI was not found in the configuration.");
}

builder.Services.AddHttpClient(
    name: "PublicApi",
    configureClient: (client) => { client.BaseAddress = new(apiUri); }
);

builder.Services.Remove(builder.Services.First(s => s.ServiceType == typeof(IHttpMessageHandlerBuilderFilter)));

WebAssemblyHost app = builder.Build();

await app.RunAsync();