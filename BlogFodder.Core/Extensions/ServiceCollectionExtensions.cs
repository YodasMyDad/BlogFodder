using BlogFodder.Core.Data;
using BlogFodder.Core.Membership.Claims;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Membership.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
                services.AddDbContextFactory<BlogFodderDbContext>(opt =>
                {
                    opt.UseSqlite(connectionString);
#if DEBUG
                    opt.EnableSensitiveDataLogging();
#endif
                });
            }

            if (databaseProvider.Equals("SqlServer"))
            {
                services.AddDbContextFactory<BlogFodderDbContext>(opt =>
                {
                    opt.UseSqlServer(connectionString);
#if DEBUG
                    opt.EnableSensitiveDataLogging();
#endif
                });
            }

            // TODO - Need to test and try other providers like MySql

#if DEBUG
            services.AddDatabaseDeveloperPageExceptionFilter();
#endif

            var identitySection = configuration.GetSection("BlogFodder:Identity");
            services.AddIdentity<User, Role>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = identitySection.GetValue<bool>("PasswordRequireDigit");
                    options.Password.RequireLowercase = identitySection.GetValue<bool>("PasswordRequireLowercase");
                    options.Password.RequireNonAlphanumeric =
                        identitySection.GetValue<bool>("PasswordRequireNonAlphanumeric");
                    options.Password.RequireUppercase = identitySection.GetValue<bool>("PasswordRequireUppercase");
                    options.Password.RequiredLength = identitySection.GetValue<int>("PasswordRequiredLength");
                    options.Password.RequiredUniqueChars = identitySection.GetValue<int>("PasswordRequiredUniqueChars");

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 6;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;

                    // Email
                    options.SignIn.RequireConfirmedAccount =
                        identitySection.GetValue<bool>("SignInRequireConfirmedAccount");
                })
                .AddEntityFrameworkStores<BlogFodderDbContext>()
                .AddUserStore<UserStore<User, Role, BlogFodderDbContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>>()
                .AddRoleStore<RoleStore<Role, BlogFodderDbContext, Guid, UserRole, RoleClaim>>()
                .AddClaimsPrincipalFactory<BlogFodderUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders();
            
            services.AddScoped<IUserEmailStore<User>, UserEmailStore>();
        }
        else
        {
            throw new Exception("Unable to find database provider in appSettings");
        }
    }
}