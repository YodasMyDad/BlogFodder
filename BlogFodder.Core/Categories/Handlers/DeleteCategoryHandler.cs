using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Categories.Handlers;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, HandlerResult<Category>>
{
    private readonly ProviderService _providerService;
    private readonly IServiceProvider _serviceProvider;
    
    public DeleteCategoryHandler(ProviderService providerService, IServiceProvider serviceProvider)
    {
        _providerService = providerService;
        _serviceProvider = serviceProvider;
    }

    public async Task<HandlerResult<Category>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new HandlerResult<Category>();
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        
        // get the category
        var categoryToDelete = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);

        if (categoryToDelete == null)
        {
            result.Success = false;
            result.AddMessage("Unable to find a category with that Id", ResultMessageType.Error);
            return result;
        }
        
        // First things first, are any posts using this category, if so return error
        var postCount = dbContext.Categories
            .Include(x => x.Posts)
            .FirstOrDefault(x => x.Id == request.CategoryId)?.Posts.Count;
        if (postCount > 0)
        {
            result.Success = false;
            result.AddMessage($"Unable to delete category as its used by {postCount} posts", ResultMessageType.Error);
            return result;            
        }

        // Get the files
        var socialImage = await dbContext.Files.FirstOrDefaultAsync(x => x.Id == categoryToDelete.SocialImageId, cancellationToken: cancellationToken);
        if (socialImage != null)
        {
            dbContext.Remove(socialImage);
            _providerService.StorageProvider?.DeleteFile(socialImage.Url);
        }
    
        // TODO - Do I need to clear the posts many to many? or will that delete the posts?    

        // Delete the post
        dbContext.Categories.Remove(categoryToDelete);

        return await dbContext.SaveChangesAndLog(categoryToDelete, result, cancellationToken);
    }
}