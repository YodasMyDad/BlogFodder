using System.Text.Json;
using BlogFodder.Core.Extensions;

namespace BlogFodder.Core.Plugins.Models;

public class PluginContentModel<T, TR>
{
    public PluginContentModel()
    {
        
    }

    public PluginContentModel(string? model, string? settings)
    {
        if (!model.IsNullOrWhiteSpace())
        {
            PluginModel = JsonSerializer.Deserialize<T>(model);
        }
        
        if (!settings.IsNullOrWhiteSpace())
        {
            PluginSettings = JsonSerializer.Deserialize<TR>(settings);
        }
    }
    
    public T? PluginModel { get; set; }
    public TR? PluginSettings { get; set; }
}