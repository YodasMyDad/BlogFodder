using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Categories.Handlers;

public class CreateUpdateCategoryHandler : IRequestHandler<CreateUpdateCategoryCommand, HandlerResult<Category>>
{
    private readonly SlugHelper _slugHelper;
    private readonly ProviderService _providerService;
    private readonly BlogFodderDbContext _dbContext;

    public CreateUpdateCategoryHandler(ProviderService providerService, BlogFodderDbContext dbContext)
    {
        _providerService = providerService;
        _dbContext = dbContext;
        _slugHelper = new SlugHelper();
    }

    public async Task<HandlerResult<Category>> Handle(CreateUpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<Category>();

        // Set any empty values like the Url
        if (request.Category.Url.IsNullOrWhiteSpace())
        {
            request.Category.Url = _slugHelper.GenerateSlug(request.Category.Name);
        }

        if (request.Category.PageTitle.IsNullOrWhiteSpace())
        {
            request.Category.PageTitle = request.Category.Name;
        }

        if (!request.IsUpdate)
        {
            request.Category.DateCreated = DateTime.UtcNow;
            request.Category.DateUpdated = DateTime.UtcNow;
        }
        else
        {
            request.Category.DateUpdated = DateTime.UtcNow;
        }

        if (_providerService.StorageProvider != null)
        {
            // See if this category already exists as we will need to remove the images
            var category = _dbContext.Categories
                .AsNoTracking()
                .Include(x => x.SocialImage)
                .FirstOrDefault(x => x.Id == request.Category.Id);

            Guid? fileIdToDelete = null;

            if (request.SocialImage != null)
            {
                // Delete the old one if there is one
                if (category?.SocialImage != null)
                {
                    fileIdToDelete = category.SocialImage.Id;
                }

                // Save the file, create a file and attach it to the user
                var fileResult = await request.SocialImage.AddFileToDb(request.Category.Id, handlerResult, _providerService, _dbContext);

                // Set the file to the user
                request.Category.SocialImage = fileResult;
            }
            else if (category?.SocialImage != null
                     && request.Category.SocialImageId == null)
            {
                // Delete the image
                fileIdToDelete = category.SocialImage.Id;
                category.SocialImageId = null;
                category.SocialImage = null;
            }

            if (category != null && fileIdToDelete != null)
            {
                var file = _dbContext.Files.FirstOrDefault(x => x.Id == fileIdToDelete);
                if (file != null)
                {
                    _dbContext.Files.Remove(file);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        
        return await _dbContext.CreateOrUpdate(request.Category, handlerResult, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);
    }
    
}