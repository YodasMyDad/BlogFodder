using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using BlogFodder.Core.Plugins;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Plugins.Plugins.ContentEditors.RichTextEditor;
using Microsoft.AspNetCore.Mvc;
using BlogFodder.Web.Models;

namespace BlogFodder.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ExtensionManager _extensionManager;

    public HomeController(ILogger<HomeController> logger, ExtensionManager extensionManager)
    {
        _logger = logger;
        _extensionManager = extensionManager;
    }

    public IActionResult Index()
    {
        var adminPages = _extensionManager.GetInstances<IPlugin>();
        ViewData["Plugins"] = adminPages;
        ViewData["DbData"] = new Dictionary<string, string> {{"RichTextEditorPlugin", JsonSerializer.Serialize(new {Testing = "I have real data boom"})}};

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]  
    public IActionResult Index(IFormCollection formCollection)
    {
        var testing = formCollection;
        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
