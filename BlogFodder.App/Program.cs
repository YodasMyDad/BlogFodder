using BlogFodder.App.Extensions;
using BlogFodder.Core;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Settings;
using BlogFodder.Plugins.Authentication.Extensions;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Extensions;
using Serilog;
using SixLabors.ImageSharp.Web.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddRazorPages();

#if DEBUG
builder.Services.AddServerSideBlazor(c => c.DetailedErrors = true);
#else
builder.Services.AddServerSideBlazor();
#endif

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddScoped<ExtensionManager>();
builder.Services.AddScoped<ProviderService>();

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(typeof(Constants).Assembly, typeof(Program).Assembly));
builder.Services.AddMudServicesWithExtensions();
builder.Services.AddMudMarkdownServices();

builder.Services.Configure<BlogFodderSettings>(builder.Configuration.GetSection(Constants.SettingsConfigName));

// TODO - Look into validation below? Need to do similar extension like the external authentication below
/*// Setup form validation
services.AddFormValidation(config =>
    config
        .AddDataAnnotationsValidation()
        .AddFluentValidation(typeof(Gab.Core.Startup).Assembly, PluginManager.Assemblies.ToArray())
);
*/

builder.Services.AddImageSharp();

// TODO - Need to uncomment when doing login stuff
//builder.Services.AddExternalAuthenticationProviders(builder.Configuration);

var app = builder.Build();

var assemblies = app.Services.DiscoverAssemblies();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
    try
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
    catch
    {
        // TODO - Need to do proper logging here!
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();