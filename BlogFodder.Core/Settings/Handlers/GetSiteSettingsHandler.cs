using BlogFodder.Core.Data;
using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Settings.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Settings.Handlers;

public class GetSiteSettingsHandler : IRequestHandler<GetSiteSettingsCommand, SiteSettings>
{
    private readonly BlogFodderDbContext _dbContext;
    private readonly ICacheService _cacheService;
    
    public GetSiteSettingsHandler(BlogFodderDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    
    public async Task<SiteSettings> Handle(GetSiteSettingsCommand request, CancellationToken cancellationToken)
    {
        return  await _cacheService.GetSetCachedItemAsync(nameof(SiteSettings), 
            async () => await _dbContext.SiteSettings.AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)) ?? new SiteSettings();
    }
}