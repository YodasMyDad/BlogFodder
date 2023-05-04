using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
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

        // Set any empty values like the Url
        if (request.Post.Url.IsNullOrWhiteSpace() || request.Post.Url.IsNullOrWhiteSpace())
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
            request.Post.DateCreated = DateTime.Now;
            request.Post.DateUpdated = DateTime.Now;
        }
        else
        {
            request.Post.DateUpdated = DateTime.Now;
        }

        if (_providerService.StorageProvider != null)
        {
            // See if this post already exsists as we will need to remove the images
            var post = _dbContext.Posts
                .Include(x => x.FeaturedImage)
                .Include(x => x.SocialImage)
                .FirstOrDefault(x => x.Id != request.Post.Id);
            
            // Profile Image - Need to save image and then create a file
            if (request.FeaturedImage != null)
            {
                // Save the file, create a file and attach it to the user
                var fileResult = await SaveImage(request.FeaturedImage, request.Post.Id, handlerResult);

                // Set the file to the user
                request.Post.FeaturedImage = fileResult;
                
                // Delete the old one if there is one
                if (post?.FeaturedImage != null)
                {
                    _dbContext.Files.Remove(post.FeaturedImage);
                }
            }
            else if(post?.FeaturedImage != null 
                    && request.Post.FeaturedImageId == null)
            {
                // Delete the image
                _dbContext.Files.Remove(post.FeaturedImage);
            }

            if (request.SocialImage != null)
            {
                // Save the file, create a file and attach it to the user
                var fileResult = await SaveImage(request.SocialImage, request.Post.Id, handlerResult);

                // Set the file to the user
                request.Post.SocialImage = fileResult;
                
                // Delete the old one if there is one
                if (post?.SocialImage != null)
                {
                    _dbContext.Files.Remove(post.SocialImage);
                }
            }
            else if(post?.SocialImage != null 
                    && request.Post.SocialImageId == null)
            {
                // Delete the image
                _dbContext.Files.Remove(post.SocialImage);
            }
        }

        if (request.IsUpdate)
        {
            // Add the new content items first
            foreach (var postContentItem in request.Post.ContentItems.Where(x => x.IsNew))
            {
                postContentItem.IsNew = false;
                _dbContext.PostContentItems.Add(postContentItem);
            }
        }
        
        return await _dbContext.CreateOrUpdate(request.Post, handlerResult, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Saves the BrowserFile as a BlogFodderFile using the set StorageProvider
    /// </summary>
    /// <param name="browserFile"></param>
    /// <param name="postId"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private async Task<BlogFodderFile?> SaveImage(IBrowserFile browserFile, Guid postId, HandlerResult<Post> result)
    {
        var fileSaveResult = await _providerService.StorageProvider!.SaveFile(browserFile, postId.ToString())
            .ConfigureAwait(false);
        if (!fileSaveResult.Success)
        {
            foreach (var errorMessage in fileSaveResult.ErrorMessages)
            {
                result.AddMessage(errorMessage, HandlerResultMessageType.Warning);
            }

            return null;
        }

        // Create the file
        var file = await _providerService.StorageProvider.ToBlogFodderFile(fileSaveResult)
            .ConfigureAwait(false);

        // Save the file first
        _dbContext.Files.Add(file);
        //await DbContext.SaveChangesAsync().ConfigureAwait(false);

        // Set the file to the user
        return file;
    }
}