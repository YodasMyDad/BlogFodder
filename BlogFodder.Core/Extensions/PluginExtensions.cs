using System.Text.Json;
using BlogFodder.Core.Plugins.Commands;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Shared.Models;
using MudBlazor;

namespace BlogFodder.Core.Extensions;

public static class PluginExtensions
{
    public static string?[] ProcessResult<T>(this CreateUpdatePluginCommand command, HandlerResult<T> result, ISnackbar snackbar)
    {
        if (result.Success)
        {
            snackbar.Add(command.ToUpdateCreateText(), Severity.Success);

            if (!command.IsUpdate)
            {
                command.IsUpdate = true;
            }

            return Array.Empty<string>();
        }

        return result.Messages.ErrorMessagesToList().ToArray();
    }

    private static string ToUpdateCreateText(this CreateUpdatePluginCommand command)
    {
        var correctText = command.IsUpdate ? "Updated" : "Created";
        return $"{correctText} Successfully";
    }
    
    public static CreateUpdatePluginCommand ReadyToSave<T,TR>(this CreateUpdatePluginCommand createUpdatePluginCommand, TR? settings, T? data)     
        where T : class
        where TR : class, IPluginSettings
    {
        createUpdatePluginCommand.Plugin!.Enabled = settings?.Enabled == true;
        if (settings != null)
        {
            createUpdatePluginCommand.Plugin!.PluginSettings = JsonSerializer.Serialize(settings, settings.GetType(), new JsonSerializerOptions {WriteIndented = false}); // Is this compact JSON?   
        }

        if (data != null)
        {
            createUpdatePluginCommand.Plugin!.PluginData = JsonSerializer.Serialize(data, data.GetType(), new JsonSerializerOptions {WriteIndented = false}); // Is this compact JSON?   
        }

        return createUpdatePluginCommand;
    }
    
    public static CreateUpdatePluginCommand ReadyToSave(this CreateUpdatePluginCommand createUpdatePluginCommand, IPluginSettings? settings)
    {
        return createUpdatePluginCommand.ReadyToSave(settings, string.Empty);
    }
}