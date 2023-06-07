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
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Settings.Handlers;

public class CreateUpdateSiteSettingsHandler : IRequestHandler<CreateUpdateSiteSettingsCommand, HandlerResult<SiteSettings>>
{
    private readonly ProviderService _providerService;
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;

    public CreateUpdateSiteSettingsHandler(ProviderService providerService,
        ICacheService cacheService, IServiceProvider serviceProvider)
    {
        _providerService = providerService;
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
    }

    public async Task<HandlerResult<SiteSettings>> Handle(CreateUpdateSiteSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var result = new HandlerResult<SiteSettings>();

        request.SiteSettings.DateUpdated = DateTime.UtcNow;

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();

        if (_providerService.StorageProvider != null)
        {
            // See if this site settings already exists as we will need to remove the images
            var siteSettings = dbContext.SiteSettings
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
                    await request.Logo.AddFileToDb(request.SiteSettings.Id, result, _providerService, dbContext);

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
                var file = dbContext.Files.FirstOrDefault(x => x.Id == fileIdToDelete);
                if (file != null)
                {
                    dbContext.Files.Remove(file);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        result = await dbContext.CreateOrUpdate(request.SiteSettings, result, !request.IsUpdate, cancellationToken)
            .ConfigureAwait(false);

        // Clear the cache
        _cacheService.ClearCachedItem(nameof(SiteSettings));

        return result;
    }
}