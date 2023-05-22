using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogFodder.Core.Shared.Middleware;

public class MissingImageMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public MissingImageMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;

        if (path.StartsWithSegments("/uploads"))
        {
            var imagePath = _env.WebRootPath + path.Value;

            if (!File.Exists(imagePath))
            {
                context.Request.Path = "/img/missing.jpg";
            }
        }

        await _next(context);
    }
}