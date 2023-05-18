using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Posts.Handlers;

public class CreateUpdatePostHandler : IRequestHandler<CreateUpdatePostCommand, HandlerResult<Post>>
{
    private readonly SlugHelper _slugHelper;
    private readonly ProviderService _providerService;
    private readonly BlogFodderDbContext _dbContext;

    public CreateUpdatePostHandler(ProviderService providerService, BlogFodderDbContext dbContext)
    {
        _providerService = providerService;
        _dbContext = dbContext;
        _slugHelper = new SlugHelper();
    }

    public async Task<HandlerResult<Post>> Handle(CreateUpdatePostCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<Post>();
        var fileIdToDelete = new List<Guid>();
        
        // Set any empty values like the Url
        if (request.Post.Url.IsNullOrWhiteSpace())
        {
            request.Post.Url = _slugHelper.GenerateSlug(request.Post.Name);
        }

        if (request.Post.MetaDescription.IsNullOrWhiteSpace())
        {
            request.Post.MetaDescription = request.Post.Excerpt;
        }

        if (request.Post.PageTitle.IsNullOrWhiteSpace())
        {
            request.Post.PageTitle = request.Post.Name;
        }

        if (!request.IsUpdate)
        {
            request.Post.DateCreated = DateTime.UtcNow;
            request.Post.DateUpdated = DateTime.UtcNow;
        }
        else
        {
            request.Post.DateUpdated = DateTime.UtcNow;
        }

        if (_providerService.StorageProvider != null)
        {
            // See if this post already exists as we will need to remove the images
            var post = _dbContext.Posts
                .Include(x => x.FeaturedImage)
                .Include(x => x.SocialImage)
                .FirstOrDefault(x => x.Id == request.Post.Id);
            
            // Profile Image - Need to save image and then create a file
            if (request.FeaturedImage != null)
            {
                // Delete the old one if there is one
                if (post?.FeaturedImage != null)
                {
                    fileIdToDelete.Add(post.FeaturedImage.Id);
                }
                
                // Save the file, create a file and attach it to the user
                var fileResult = await request.FeaturedImage.SaveFileToDb(request.Post.Id, handlerResult, _providerService, _dbContext);

                // Set the file to the user
                request.Post.FeaturedImage = fileResult;
                
            }
            else if(post?.FeaturedImage != null 
                    && request.Post.FeaturedImageId == null)
            {
                // Delete the image
                fileIdToDelete.Add(post.FeaturedImage.Id);
                post.FeaturedImageId = null;
                post.FeaturedImage = null;
            }

            if (request.SocialImage != null)
            {
                // Delete the old one if there is one
                if (post?.SocialImage != null)
                {
                    fileIdToDelete.Add(post.SocialImage.Id);
                }
                
                // Save the file, create a file and attach it to the user
                var fileResult = await request.SocialImage.SaveFileToDb(request.Post.Id, handlerResult, _providerService, _dbContext);

                // Set the file to the user
                request.Post.SocialImage = fileResult;
                
            }
            else if(post?.SocialImage != null 
                    && request.Post.SocialImageId == null)
            {
                // Delete the image
                fileIdToDelete.Add(post.SocialImage.Id);
                post.SocialImageId = null;
                post.SocialImage = null;
            }

            if (post != null && fileIdToDelete.Any())
            {
                foreach (var guid in fileIdToDelete)
                {
                    var file = _dbContext.Files.FirstOrDefault(x => x.Id == guid);
                    if (file != null)
                    {
                        _dbContext.Files.Remove(file);
                    }
                }
                
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        if (request.IsUpdate)
        {
            // Add the new content items first
            foreach (var postContentItem in request.Post.ContentItems)
            {
                if (postContentItem.IsNew)
                {
                    postContentItem.IsNew = false;
                    _dbContext.PostContentItems.Add(postContentItem);   
                }
                else
                {
                    postContentItem.IsNew = false;
                    _dbContext.PostContentItems.Update(postContentItem);
                }
            }
        }
        
        return await _dbContext.CreateOrUpdate(request.Post, handlerResult, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
    }
}