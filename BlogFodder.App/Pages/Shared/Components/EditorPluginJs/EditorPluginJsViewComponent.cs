using BlogFodder.Core;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogFodder.App.Pages.Shared.Components.EditorPluginJs;

/// <summary>
/// Article block view component
/// </summary>
[ViewComponent(Name = "EditorPluginJs")]
public class EditorPluginJsViewComponent : ViewComponent
{
    private readonly ExtensionManager _extensionManager;

    public EditorPluginJsViewComponent(ExtensionManager extensionManager)
    {
        _extensionManager = extensionManager;
    }

    public IViewComponentResult Invoke()
    {
        var plugins = _extensionManager.GetInstances<IEditorPlugin>(true);

        var currentUrl = HttpContext.Request.Path.Value;
        var startsWithAdmin =
            currentUrl?.StartsWith(Constants.BackOffice.BackOfficeUrlDenoter, StringComparison.OrdinalIgnoreCase) ??
            false;
        var jsFiles = new List<string>();

        // Filter the content based on the URL
        if (startsWithAdmin)
        {
            // TODO - Should these be in the appSettings?
            // Add mud blazor files
            jsFiles.Add("_content/MudBlazor/MudBlazor.min.js");

            foreach (var plugin in plugins)
            {
                if (plugin.Value.Editor?.JsFiles != null)
                {
                    foreach (var jsFile in plugin.Value.Editor.JsFiles)
                    {
                        jsFiles.Add(jsFile);
                    }
                }
            }
        }
        else
        {
            foreach (var plugin in plugins)
            {
                if (plugin.Value.Content?.JsFiles != null)
                {
                    foreach (var jsFile in plugin.Value.Content.JsFiles)
                    {
                        jsFiles.Add(jsFile);
                    }
                }
            }
        }

        return View(jsFiles);
    }
}