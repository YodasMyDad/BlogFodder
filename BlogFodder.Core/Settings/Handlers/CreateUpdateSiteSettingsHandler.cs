using AutoMapper;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
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
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public CreateUpdateSiteSettingsHandler(ProviderService providerService,
        ICacheService cacheService, IServiceProvider serviceProvider, IMapper mapper)
    {
        _providerService = providerService;
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public async Task<HandlerResult<SiteSettings>> Handle(CreateUpdateSiteSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var result = new HandlerResult<SiteSettings>();

        request.SiteSettings.DateUpdated = DateTime.UtcNow;

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();

        // See if this site settings already exists as we will need to remove the images
        var siteSettings = dbContext.SiteSettings
            .Include(x => x.Logo)
            .FirstOrDefault();

        var oldLogoId = siteSettings?.LogoId;
            
        siteSettings ??= new SiteSettings();

        _mapper.Map(request.SiteSettings, siteSettings);
            
        if (!request.IsUpdate)
        {
            dbContext.SiteSettings.Add(siteSettings);
        }

        // If new images are provided, save them and update the post properties
        if (request.Logo != null)
        {
            var logo =
                await request.Logo.AddFileToDb(request.SiteSettings.Id, result, _providerService, dbContext);
            siteSettings.Logo = logo;
            siteSettings.LogoId = logo?.Id;

            // If an old image existed, delete it
            if (oldLogoId != null)
            {
                var oldLogo = await dbContext.Files.FindAsync(new object?[] {oldLogoId},
                    cancellationToken: cancellationToken);
                if (oldLogo != null)
                {
                    dbContext.Files.Remove(oldLogo);
                }
            }
        }
  

        result = await dbContext.SaveChangesAndLog(siteSettings, result, cancellationToken)
            .ConfigureAwait(false);

        // Clear the cache
        _cacheService.ClearCachedItem(nameof(SiteSettings));

        return result;
    }
}