using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

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
            request.Post.DateCreated = DateTime.Now;
            request.Post.DateUpdated = DateTime.Now;
        }
        else
        {
            request.Post.DateUpdated = DateTime.Now;
        }

        if (_providerService.StorageProvider != null)
        {
            // Profile Image - Need to save image and then create a gabfile
            if (request.FeaturedImage != null)
            {
                // TODO - Need to see if we are deleting an old image

                // Save the file, create a gab file and attach it to the user
                var fileResult = await SaveImage(request.FeaturedImage, request.Post.Id, handlerResult);

                // Set the file to the user
                request.Post.FeaturedImage = fileResult;
            }

            if (request.SocialImage != null)
            {
                // TODO - Need to see if we are deleting an old image

                // Save the file, create a gab file and attach it to the user
                var fileResult = await SaveImage(request.SocialImage, request.Post.Id, handlerResult);

                // Set the file to the user
                request.Post.SocialImage = fileResult;
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