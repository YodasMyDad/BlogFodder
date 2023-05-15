using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Plugins.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Settings.Models;
using BlogFodder.Core.Shared.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Settings.Handlers;

public class CreateUpdateSiteSettingsHandler : IRequestHandler<CreateUpdateSiteSettingsCommand, HandlerResult<SiteSettings>>
{
    private readonly ProviderService _providerService;
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public CreateUpdateSiteSettingsHandler(ProviderService providerService, BlogFodderDbContext dbContext,
        ICacheService cacheService)
    {
        _providerService = providerService;
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<HandlerResult<SiteSettings>> Handle(CreateUpdateSiteSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var result = new HandlerResult<SiteSettings>();

        request.SiteSettings.DateUpdated = DateTime.UtcNow;

        if (_providerService.StorageProvider != null)
        {
            // See if this site settings already exists as we will need to remove the images
            var siteSettings = _dbContext.SiteSettings
                .Include(x => x.Logo)
                .FirstOrDefault();

            Guid? fileIdToDelete = null;
            if (request.Logo != null)
            {
                // Delete the old one if there is one
                if (siteSettings?.Logo != null)
                {
                    fileIdToDelete = siteSettings.Logo.Id;
                }

                // Save the file, create a file and attach it to the user
                var fileResult =
                    await request.Logo.SaveFileToDb(request.SiteSettings.Id, result, _providerService, _dbContext);

                // Set the file to the user
                request.SiteSettings.Logo = fileResult;
            }
            else if (siteSettings?.Logo != null
                     && request.SiteSettings.LogoId == null)
            {
                // Delete the image
                fileIdToDelete = siteSettings.Logo.Id;
                siteSettings.LogoId = null;
                siteSettings.Logo = null;
            }

            if (siteSettings != null && fileIdToDelete != null)
            {
                var file = _dbContext.Files.FirstOrDefault(x => x.Id == fileIdToDelete);
                if (file != null)
                {
                    _dbContext.Files.Remove(file);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        result = await _dbContext.CreateOrUpdate(request.SiteSettings, result, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);

        // Clear the cache
        _cacheService.ClearCachedItem(nameof(SiteSettings));

        return result;
    }
}