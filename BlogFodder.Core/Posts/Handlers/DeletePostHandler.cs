using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Posts.Handlers;

public class DeletePostHandler: IRequestHandler<DeletePostCommand, HandlerResult<Post>>
{
    private readonly BlogFodderDbContext _dbContext;

    public DeletePostHandler(BlogFodderDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<HandlerResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<Post>();
        
        // get the post and the content items
        var postToDelete = await _dbContext.Posts
            .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);

        if (postToDelete == null)
        {
            result.Success = false;
            result.AddMessage("Unable to find a post with that Id", HandlerResultMessageType.Error);
            return result;
        }

        // Get the files
        var socialImage = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == postToDelete.SocialImageId, cancellationToken: cancellationToken);
        var featuredImage = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == postToDelete.FeaturedImageId, cancellationToken: cancellationToken);
        if (socialImage != null)
        {
            _dbContext.Remove(socialImage);   
        }
        if (featuredImage != null)
        {
            _dbContext.Remove(featuredImage);   
        }

        // Delete the content items
        var contentItemsToDelete =  _dbContext.PostContentItems.Where(x => x.PostId == postToDelete.Id);
        if (contentItemsToDelete.Any())
        {
            foreach (var postContentItem in contentItemsToDelete)
            {
                _dbContext.Remove(postContentItem);   
            }
        }

        // Delete the post
        _dbContext.Posts.Remove(postToDelete);

        return await _dbContext.SaveChangesAndLog(result, cancellationToken);
    }
}