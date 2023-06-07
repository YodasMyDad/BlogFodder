using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Posts.Handlers;

public class DeletePostHandler: IRequestHandler<DeletePostCommand, HandlerResult<Post>>
{
    private readonly ProviderService _providerService;
    private readonly IServiceProvider _serviceProvider;
    
    public DeletePostHandler(ProviderService providerService, IServiceProvider serviceProvider)
    {
        _providerService = providerService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<HandlerResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<Post>();
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        
        // get the post and the content items
        var postToDelete = await dbContext.Posts
            .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);

        if (postToDelete == null)
        {
            result.Success = false;
            result.AddMessage("Unable to find a post with that Id", ResultMessageType.Error);
            return result;
        }

        // Get the files
        var socialImage = await dbContext.Files.FirstOrDefaultAsync(x => x.Id == postToDelete.SocialImageId, cancellationToken: cancellationToken);
        var featuredImage = await dbContext.Files.FirstOrDefaultAsync(x => x.Id == postToDelete.FeaturedImageId, cancellationToken: cancellationToken);
        if (socialImage != null)
        {
            dbContext.Remove(socialImage);
            _providerService.StorageProvider?.DeleteFile(socialImage.Url);
        }
        if (featuredImage != null)
        {
            dbContext.Remove(featuredImage);   
            _providerService.StorageProvider?.DeleteFile(featuredImage.Url);
        }

        // Delete the content items
        var contentItemsToDelete =  dbContext.PostContentItems.Where(x => x.PostId == postToDelete.Id);
        if (contentItemsToDelete.Any())
        {
            foreach (var postContentItem in contentItemsToDelete)
            {
                dbContext.Remove(postContentItem);   
            }
        }

        // Delete the post
        dbContext.Posts.Remove(postToDelete);

        return await dbContext.SaveChangesAndLog(result, cancellationToken);
    }
}