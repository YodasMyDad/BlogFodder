using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Media.Mapping;

/// <summary>
/// The database mapping file for BlogFodderFile 
/// </summary>
public class BlogFodderFileDbMapping : IEntityTypeConfiguration<BlogFodderFile>
{
    public void Configure(EntityTypeBuilder<BlogFodderFile> builder)
    {
        builder.ToTable("BlogFodderFiles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Url).HasMaxLength(600);
        builder.Property(x => x.FileType).IsRequired();
        builder.Property(x => x.ItemId).HasMaxLength(100);
        builder.Property(x => x.DateCreated).IsRequired();
        builder.Property(x => x.IsTemp).IsRequired();
        builder.Property(e => e.ExtendedData).ToJsonConversion(4000);
        builder.HasIndex(x => x.ItemId).HasDatabaseName("IX_BlogFodderFileItemId");
        builder.HasIndex(x => x.DateCreated).HasDatabaseName("IX_BlogFodderFileDateCreated");
        builder.HasIndex(x => x.IsTemp).HasDatabaseName("IX_BlogFodderFileIsTemp");
    }
}