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
        builder.HasIndex(x => x.PluginAlias).HasDatabaseName("IX_PluginAlias");
    }
}