using System.Text.Json;
using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Extensions;

public static class SettingsExtensions
{
    public static T? ToType<T>(this GlobalSettings? globalSettings)
    {
        if (globalSettings != null)
        {
            if (!globalSettings.Data.IsNullOrWhiteSpace())
            {
                return JsonSerializer.Deserialize<T>(globalSettings.Data);
            }
        }
        return default;
    }
}