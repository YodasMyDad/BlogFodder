using BlogFodder.Core.Extensions;
using BlogFodder.Core.Settings.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Settings.Mapping;

public class SiteSettingsDbMapping : IEntityTypeConfiguration<SiteSettings>
{
    public void Configure(EntityTypeBuilder<SiteSettings> builder)
    {
        builder.ToTable("BlogFodderSiteSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.DateUpdated).IsRequired();
        builder.Property(x => x.SiteName).IsRequired().HasMaxLength(400);
        builder.Property(x => x.DefaultPageTitle).IsRequired().HasMaxLength(150);
        builder.Property(x => x.DefaultMetaDescription).IsRequired().HasMaxLength(400);
        
        builder.Property(x => x.ExtendedData).ToJsonConversion(4000);
    }
}