using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;

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
            
            /*
            // Profile Image - Need to save image and then create a gabfile
            if (FeaturedImage != null)
            {
                // Save the file, create a gab file and attach it to the user
                if (ProviderService.StorageProvider != null)
                {
                    var fileSaveResult = await ProviderService.StorageProvider
                        .SaveFile(FeaturedImage, PostCommand.Post.Id.ToString()).ConfigureAwait(false);
                    if (!fileSaveResult.Success)
                    {
                        foreach (var errorMessage in fileSaveResult.ErrorMessages)
                        {
                            Snackbar.Add(errorMessage, Severity.Error);
                        }
                    }

                    // Create the gabfile
                    var file = await ProviderService.StorageProvider.ToBlogFodderFile(fileSaveResult)
                        .ConfigureAwait(false);

                    // Save the file first
                    DbContext.Files.Add(file);
                    //await DbContext.SaveChangesAsync().ConfigureAwait(false);

                    // Set the file to the user
                    PostCommand.Post.FeaturedImage = file;
                }
            }

            if (SocialImage != null)
            {
                // Save the file, create a gab file and attach it to the user
                if (ProviderService.StorageProvider != null)
                {
                    var fileSaveResult = await ProviderService.StorageProvider.SaveFile(SocialImage, PostCommand.Post.Id.ToString())
                        .ConfigureAwait(false);
                    if (!fileSaveResult.Success)
                    {
                        foreach (var errorMessage in fileSaveResult.ErrorMessages)
                        {
                            Snackbar.Add(errorMessage, Severity.Error);
                        }
                    }

                    // Create the gabfile
                    var file = await ProviderService.StorageProvider.ToBlogFodderFile(fileSaveResult)
                        .ConfigureAwait(false);

                    // Save the file first
                    DbContext.Files.Add(file);
                    //await DbContext.SaveChangesAsync().ConfigureAwait(false);

                    // Set the file to the user
                    PostCommand.Post.SocialImage = file;
                }
            }
            */
            
            if (request.IsUpdate)
            {
                // Add the new content items first
                foreach (var postContentItem in request.Post.ContentItems.Where(x => x.IsNew))
                {
                    postContentItem.IsNew = false;
                    _dbContext.PostContentItems.Add(postContentItem);
                }
            }

            //_dbContext.Posts.Add(request.Post);
            return await _dbContext.CreateOrUpdate(request.Post, !request.IsUpdate, cancellationToken).ConfigureAwait(false);
    }
}