using BlogFodder.Core.Plugins;
using Microsoft.AspNetCore.Mvc;

namespace BlogFodder.Web.Controllers;

public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private readonly ExtensionManager _extensionManager;

    public PostController(ILogger<PostController> logger, ExtensionManager extensionManager)
    {
        _logger = logger;
        _extensionManager = extensionManager;
    }

    public IActionResult CreatePost()
    {
        return View();
    }

}