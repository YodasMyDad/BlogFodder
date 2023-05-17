using BlogFodder.Core.Plugins.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Plugins.Mapping;

public class PluginSettingsDbMapping : IEntityTypeConfiguration<PluginSettings>
{
    public void Configure(EntityTypeBuilder<PluginSettings> builder)
    {
        builder.ToTable("BlogFodderPluginsGlobalSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Alias).HasMaxLength(1000);
        
        builder.HasIndex(x => x.Alias).HasDatabaseName("IX_GlobalSettingsAlias");
    }
}