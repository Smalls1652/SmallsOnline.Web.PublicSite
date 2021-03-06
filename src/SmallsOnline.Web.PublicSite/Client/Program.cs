using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using SmallsOnline.Web.PublicSite.Client;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(
    sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);

builder.Services.AddHttpClient(
    name: "BaseAppClient",
    configureClient: (client) =>
    {
        client.BaseAddress = new(builder.HostEnvironment.BaseAddress);
    }
);

string apiUri = builder.Configuration.GetValue<string>("ApiUri");

builder.Services.AddHttpClient(
    name: "PublicApi",
    configureClient: (client) =>
    {
        client.BaseAddress = new(apiUri);
    }
);

WebAssemblyHost app = builder.Build();

await app.RunAsync();