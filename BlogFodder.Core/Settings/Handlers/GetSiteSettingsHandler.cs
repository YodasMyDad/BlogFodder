using BlogFodder.Core.Data;
using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Settings.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Settings.Handlers;

public class GetSiteSettingsHandler : IRequestHandler<GetSiteSettingsCommand, SiteSettings>
{
    private readonly ICacheService _cacheService;
    private readonly IServiceProvider _serviceProvider;
    public GetSiteSettingsHandler(ICacheService cacheService, IServiceProvider serviceProvider)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<SiteSettings> Handle(GetSiteSettingsCommand request, CancellationToken cancellationToken)
    {
        return  await _cacheService.GetSetCachedItemAsync(nameof(SiteSettings), 
            async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
                return await dbContext.SiteSettings.Include(x => x.Logo).AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            }) ?? new SiteSettings();
    }
}