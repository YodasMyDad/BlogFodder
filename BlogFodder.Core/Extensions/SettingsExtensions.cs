using System.Text.Json;
using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Extensions;

public static class SettingsExtensions
{
    public static T? ToType<T>(this GlobalPluginSettings? globalSettings)
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

    public static T? ToType<T>(this string data)
    {
        if (!data.IsNullOrWhiteSpace())
        {
            return JsonSerializer.Deserialize<T>(data);
        }

        return default;
    }
}