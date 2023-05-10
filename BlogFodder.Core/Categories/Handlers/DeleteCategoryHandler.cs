using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Categories.Handlers;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, HandlerResult<Category>>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ProviderService _providerService;

    public DeleteCategoryHandler(BlogFodderDbContext dbContext, ProviderService providerService)
    {
        _dbContext = dbContext;
        _providerService = providerService;
    }

    public async Task<HandlerResult<Category>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<Category>();
        

        
        // get the category
        var categoryToDelete = await _dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);

        if (categoryToDelete == null)
        {
            result.Success = false;
            result.AddMessage("Unable to find a category with that Id", HandlerResultMessageType.Error);
            return result;
        }
        
        // First things first, are any posts using this category, if so return error
        var postCount = _dbContext.Categories
            .Include(x => x.Posts)
            .FirstOrDefault(x => x.Id == request.CategoryId)?.Posts.Count;
        if (postCount > 0)
        {
            result.Success = false;
            result.AddMessage($"Unable to delete category as its used by {postCount} posts", HandlerResultMessageType.Error);
            return result;            
        }

        // Get the files
        var socialImage = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == categoryToDelete.SocialImageId, cancellationToken: cancellationToken);
        if (socialImage != null)
        {
            _dbContext.Remove(socialImage);
            _providerService.StorageProvider?.DeleteFile(socialImage.Url);
        }
    
        // TODO - Do I need to clear the posts many to many? or will that delete the posts?    

        // Delete the post
        _dbContext.Categories.Remove(categoryToDelete);

        return await _dbContext.SaveChangesAndLog(result, cancellationToken);
    }
}