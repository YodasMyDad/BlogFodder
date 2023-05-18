using Akismet;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Plugins.Comments;

public class PostCommentsStartup : IStartupPlugin
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        var akismetSpiKey = configuration.GetSection("PostCommentsPlugin")["AkismetApiKey"];
        if (!akismetSpiKey.IsNullOrWhiteSpace())
        {
            services.AddSingleton(
                new AkismetClient(
                    configuration.GetSection("PostCommentsPlugin")["AkismetApiKey"] ?? "",
                    new Uri(configuration.GetSection("PostCommentsPlugin")["Url"] ?? ""),
                    configuration.GetSection("PostCommentsPlugin")["ApplicationName"] ?? "")
            );   
        }
    }
}