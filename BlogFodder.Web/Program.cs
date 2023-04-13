using System.Reflection;
using BlogFodder.Core;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Identity.Models;
using BlogFodder.Core.Plugins;
using BlogFodder.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddServerSideBlazor();
builder.Services.AddControllersWithViews();
builder.Services.AddDatabase(builder.Configuration);

/*.ConfigureApplicationPartManager(manager =>
{
// This won't work
foreach (var assembly in ExtensionManager.GetAssemblies(x => x.FullName!.Contains("BlogFodder.Plugins")))
{
    if (assembly != null)
    {
        var assemblyPart = new AssemblyPart(assembly);
        var fileProvider = new EmbeddedFileProvider(assembly);
        manager.ApplicationParts.Add(assemblyPart);           
        builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options => options.FileProviders.Add(fileProvider));
        //manager.ApplicationParts.Add(new CompiledRazorAssemblyPart(assembly));                    
    }
}
});*/

builder.Services.AddScoped<ExtensionManager>();

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(typeof(Constants).Assembly, typeof(Program).Assembly));

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

//TODO - We need to add 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name : "areas",
    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();
app.MapBlazorHub();

app.Run();