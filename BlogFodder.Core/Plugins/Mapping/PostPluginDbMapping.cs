using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Plugins.Mapping;

public class PluginDbMapping : IEntityTypeConfiguration<Plugin>
{
    public void Configure(EntityTypeBuilder<Plugin> builder)
    {
        builder.ToTable("BlogFodderPlugins");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.PluginAlias).HasMaxLength(600);
        builder.Property(x => x.PluginDisplayAreas).ToJsonConversion(3000);
        
        builder.Ignore(x => x.GlobalSettings);
        builder.Ignore(x => x.IsNew);

        builder.HasIndex(x => x.PluginAlias).HasDatabaseName("IX_PluginAlias");
    }
}