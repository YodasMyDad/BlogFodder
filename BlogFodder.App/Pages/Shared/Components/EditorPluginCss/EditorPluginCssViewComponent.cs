using BlogFodder.Core;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlogFodder.App.Pages.Shared.Components.EditorPluginCss;

/// <summary>
/// Article block view component
/// </summary>
[ViewComponent(Name = "EditorPluginCss")]
public class EditorPluginCssViewComponent : ViewComponent
{
    private readonly ExtensionManager _extensionManager;
    private readonly BlogFodderSettings _optionsSnapshot;

    public EditorPluginCssViewComponent(ExtensionManager extensionManager, IOptionsSnapshot<BlogFodderSettings> optionsSnapshot)
    {
        _extensionManager = extensionManager;
        _optionsSnapshot = optionsSnapshot.Value;
    }

    public IViewComponentResult Invoke()
    {
        var plugins = _extensionManager.GetInstances<IEditorPlugin>(true);
        
        var currentUrl = HttpContext.Request.Path.Value;
        var startsWithAdmin =
            currentUrl?.StartsWith(Constants.BackOffice.BackOfficeUrlDenoter, StringComparison.OrdinalIgnoreCase) ??
            false;
        var cssFiles = new List<string>();

        if (startsWithAdmin)
        {
            // TODO - Should these be in the appSettings?
            // Add mud blazor files
            cssFiles.Add("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap");
            cssFiles.Add("_content/MudBlazor/MudBlazor.min.css");
            
            foreach (var plugin in plugins)
            {
                if (plugin.Value.Editor?.CssFiles != null)
                {
                    foreach (var cssFile in plugin.Value.Editor.CssFiles)
                    {
                        cssFiles.Add(cssFile);
                    }
                }
            }
        }
        else
        {
            cssFiles.AddRange(_optionsSnapshot.FrontEnd.Styles);
            foreach (var plugin in plugins)
            {
                if (plugin.Value.Content?.CssFiles != null)
                {
                    foreach (var cssFile in plugin.Value.Content.CssFiles)
                    {
                        cssFiles.Add(cssFile);
                    }
                }
            }
        }
        

        return View(cssFiles);
    }
}