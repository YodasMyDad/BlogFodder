using BlogFodder.Core.Plugins;
using BlogFodder.Web.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServerSideBlazor();
builder.Services.AddControllersWithViews();

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

var app = builder.Build();

app.Services.DiscoverAssemblies();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //https://stackoverflow.com/questions/50762385/loading-razor-class-libraries-as-plugins
    //https://stackoverflow.com/questions/51003853/how-compiledrazorassemblypart-should-be-used-to-load-razor-views
    
    //Load this project and copy it
    //https://github.com/DominikAmon/RclIssueDemo/blob/master/RclDemo.WebApplication/Startup.cs
    


    /*var dbContext = services.GetRequiredService<AppDbContext>();
    
    try
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();   
        }
    }
    catch { }*/
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();

app.Run();