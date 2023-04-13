using BlogFodder.Core.Posts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Posts.Mapping;

public class PostContentItemDbMapping : IEntityTypeConfiguration<PostContentItem>
{
    public void Configure(EntityTypeBuilder<PostContentItem> builder)
    {
        builder.ToTable("BlogFodderPostContentItems");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.HasOne(d => d.Post)
            .WithMany(p => p.ContentItems)
            .HasForeignKey(d => d.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.PluginAlias).HasMaxLength(500);
    }
}