using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BlogFodder.Core.Data;

public class BlogFodderContextFactory : IDesignTimeDbContextFactory<BlogFodderDbContext>
{
    public BlogFodderDbContext CreateDbContext(string[] args)
    {
        // Get the environment variable
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BlogFodder.App"))
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appSettings.{environment}.json", optional: true)
            .Build();
        
        // Get the connection string and provider from configuration
        var section = configuration.GetSection("BlogFodder");
        var connectionString = section.GetValue<string>("ConnectionString");
        var databaseProvider = section.GetValue<string>("DatabaseProvider");

        // Configure the DbContext based on the provider
        var optionsBuilder = new DbContextOptionsBuilder<BlogFodderDbContext>();
        if (databaseProvider == "Sqlite")
        {
            optionsBuilder.UseSqlite(connectionString);
        }
        else if (databaseProvider == "SqlServer")
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        return new BlogFodderDbContext(optionsBuilder.Options);
    }
}
