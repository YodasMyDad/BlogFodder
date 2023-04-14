using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlogFodder.App.Data;
using BlogFodder.App.Extensions;
using BlogFodder.Core;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddRazorPages();
    /*.ConfigureApplicationPartManager(manager =>
{
    foreach (var assembly in ExtensionManager.GetAssemblies(x => x.FullName!.StartsWith("BlogFodder.Plugins", StringComparison.OrdinalIgnoreCase)))
    {
        if (assembly != null)
        {
            /*var assemblyPart = new AssemblyPart(assembly);
            var fileProvider = new EmbeddedFileProvider(assembly);
            manager.ApplicationParts.Add(assemblyPart);           
            builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options => options.FileProviders.Add(fileProvider));#1#
            manager.ApplicationParts.Add(new CompiledRazorAssemblyPart(assembly));                    
        }
    }
});*/
builder.Services.AddServerSideBlazor();
builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddScoped<ExtensionManager>();

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(typeof(Constants).Assembly, typeof(Program).Assembly));
builder.Services.AddMudServices();
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