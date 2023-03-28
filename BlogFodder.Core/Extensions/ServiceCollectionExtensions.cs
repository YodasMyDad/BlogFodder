using BlogFodder.Core.Data;
using BlogFodder.Core.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("BlogFodder");
        var connectionString = section.GetValue<string>("ConnectionString");
        var databaseProvider = section.GetValue<string>("DatabaseProvider");
        if (databaseProvider != null)
        {
            if (databaseProvider.Equals("Sqlite"))
            {
                services.AddDbContext<BlogFodderDbContext>(o => o.UseSqlite(connectionString));
            }
            
            // TODO - Need to test and try these providers
            /*if (databaseProvider.Equals("SqlServer"))
            {
                services.AddDbContext<BlogFodderDbContext>(o => o.UseSqlServer(connectionString));
            }
            
            if (databaseProvider.Equals("Postgres"))
            {
                services.AddDbContext<BlogFodderDbContext>(o => o.UseNpgsql(connectionString));
            }*/
            
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BlogFodderDbContext>();
        }
        else
        {
            throw new Exception("Unable to find database provider in appSettings");
        }
    }
}