using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlogFodder.Core.Data;

public class BlogFodderContextFactory : IDesignTimeDbContextFactory<BlogFodderDbContext>
{
    public BlogFodderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlogFodderDbContext>();
        optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");

        return new BlogFodderDbContext(optionsBuilder.Options);
    }
}
