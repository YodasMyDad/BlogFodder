using BlogFodder.Core.Extensions;
using BlogFodder.Core.Posts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Posts.Mapping;

public class PostDbMapping : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("BlogFodderPosts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(1000);
        builder.Property(x => x.DateCreated).IsRequired();
        builder.Property(x => x.DateUpdated).IsRequired();
        builder.Property(x => x.Excerpt).HasMaxLength(3000);
        builder.Property(x => x.FeaturedImage).HasMaxLength(1000);
        
        builder.Property(x => x.PageTitle).HasMaxLength(100);
        builder.Property(x => x.MetaDescription).HasMaxLength(350);
        builder.Property(x => x.SocialImage).HasMaxLength(1000);
        builder.Property(x => x.Url).HasMaxLength(500);
        builder.Property(x => x.ExtendedData).ToJsonConversion(4000);
    }
}