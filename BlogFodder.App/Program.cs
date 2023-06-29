using Blazored.Toast;
using BlogFodder.Core;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Settings;
using BlogFodder.Core.Settings.Models;
using BlogFodder.Core.Shared.Middleware;
using BlogFodder.Core.Shared.Services;
using BlogFodder.Plugins;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddImageSharp();
builder.Services.AddAuthentication();

builder.Services.AddScoped<ExtensionManager>();
builder.Services.AddScoped<ProviderService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<SignInManager<User>, BlogFodderSignInManager>();
builder.Services.AddMudServicesWithExtensions();
builder.Services.AddBlazoredToast();

builder.Services.Configure<BlogFodderSettings>(builder.Configuration.GetSection(Constants.SettingsConfigName));
builder.Services.AddMvc();
builder.Services.EnableBlogFodderPlugins(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
    var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
    try
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
        
        await dbContext.SeedData(mediatr);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error during startup trying to do Db migrations");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<MissingImageMiddleware>();
app.UseImageSharp();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();