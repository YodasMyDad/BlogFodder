using System.Reflection;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Identity.Models;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Settings.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Data;

public class BlogFodderDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public BlogFodderDbContext(DbContextOptions<BlogFodderDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PostContentItem> PostContentItems => Set<PostContentItem>();
    public DbSet<Plugin> Plugins => Set<Plugin>();
    public DbSet<BlogFodderFile> Files => Set<BlogFodderFile>();
    public DbSet<PluginSettings> PluginSettings => Set<PluginSettings>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        // The apply configurations from assembly isn't working for the identity models
        modelBuilder.Entity<User>().ToTable("BlogFodderUsers");
        modelBuilder.Entity<Role>().ToTable("BlogFodderRoles");
        modelBuilder.Entity<UserClaim>().ToTable("BlogFodderUserClaims");
        modelBuilder.Entity<UserRole>().ToTable("BlogFodderUserRoles");
        modelBuilder.Entity<UserLogin>().ToTable("BlogFodderUserLogins");
        modelBuilder.Entity<RoleClaim>().ToTable("BlogFodderRoleClaims");
        modelBuilder.Entity<UserToken>().ToTable("BlogFodderUserTokens");
    }
}