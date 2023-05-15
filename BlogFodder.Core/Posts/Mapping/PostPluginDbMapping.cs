using BlogFodder.Core.Posts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Posts.Mapping;

public class PostPluginDbMapping : IEntityTypeConfiguration<PostPlugin>
{
    public void Configure(EntityTypeBuilder<PostPlugin> builder)
    {
        builder.ToTable("BlogFodderPostPlugins");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.HasOne(d => d.Post)
            .WithMany(p => p.Plugins)
            .HasForeignKey(d => d.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.PluginAlias).HasMaxLength(600);
        
        builder.Ignore(x => x.GlobalSettings);
        builder.Ignore(x => x.IsNew);
        
        builder.HasIndex(x => x.PluginAlias).HasDatabaseName("IX_PostPluginPluginAlias");
    }
}