using System.Reflection;
using BlogFodder.Core.Identity.Models;
using BlogFodder.Core.Media;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Posts.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Data;

public class BlogFodderDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public BlogFodderDbContext(DbContextOptions<BlogFodderDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostContentItem> PostContentItems => Set<PostContentItem>();
    public DbSet<BlogFodderFile> Files => Set<BlogFodderFile>();
    
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