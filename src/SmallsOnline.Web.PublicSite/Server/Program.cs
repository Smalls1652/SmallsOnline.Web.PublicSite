using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped(
    sp => new HttpClient { BaseAddress = new Uri("https://smalls.online/") }
);

builder.Services.AddHttpClient(
    name: "BaseAppClient",
    configureClient: (client) =>
    {
        client.BaseAddress = new("https://smalls.online/");
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();
