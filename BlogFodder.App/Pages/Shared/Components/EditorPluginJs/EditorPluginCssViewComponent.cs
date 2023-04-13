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
        var plugins = _extensionManager.GetInstances<IPlugin>(true);
        var jsFiles = new List<string>();
        foreach (var plugin in plugins)
        {
            if (plugin?.Editor?.JsFiles != null)
            {
                foreach (var jsFile in plugin.Editor.JsFiles)
                {
                    jsFiles.Add(jsFile);
                }
            }
        }

        return View(jsFiles);
    }
}