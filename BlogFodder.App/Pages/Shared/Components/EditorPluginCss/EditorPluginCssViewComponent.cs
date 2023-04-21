using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogFodder.App.Pages.Shared.Components.EditorPluginCss;

/// <summary>
/// Article block view component
/// </summary>
[ViewComponent(Name = "EditorPluginCss")]
public class EditorPluginCssViewComponent : ViewComponent
{
    private readonly ExtensionManager _extensionManager;

    public EditorPluginCssViewComponent(ExtensionManager extensionManager)
    {
        _extensionManager = extensionManager;
    }

    public IViewComponentResult Invoke()
    {
        var plugins = _extensionManager.GetInstances<IEditorPlugin>(true);
        var cssFiles = new List<string>();
        foreach (var plugin in plugins)
        {
            if (plugin?.Editor?.CssFiles != null)
            {
                foreach (var cssFile in plugin.Editor.CssFiles)
                {
                    cssFiles.Add(cssFile);
                }
            }
        }

        return View(cssFiles);
    }
}