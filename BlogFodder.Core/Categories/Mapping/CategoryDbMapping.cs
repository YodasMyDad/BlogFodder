using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Categories.Mapping;

public class CategoryDbMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("BlogFodderCategories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(1000);
        builder.Property(x => x.DateCreated).IsRequired();
        builder.Property(x => x.DateUpdated).IsRequired();
        builder.Property(x => x.PageTitle).HasMaxLength(100);
        builder.Property(x => x.MetaDescription).HasMaxLength(350);
        builder.Property(x => x.Url).HasMaxLength(500);
        builder.Property(x => x.ExtendedData).ToJsonConversion(4000);
        
        // Many to many
        builder.HasMany(e => e.Posts)
                .WithMany(e => e.Categories);
    }
}