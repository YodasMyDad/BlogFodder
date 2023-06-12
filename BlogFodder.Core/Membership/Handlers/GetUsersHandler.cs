using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Membership.Handlers;

public class GetUsersHandler : IRequestHandler<GetUsersCommand, List<User>>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICacheService _cacheService;

    public GetUsersHandler(IServiceProvider serviceProvider, ICacheService cacheService)
    {
        _serviceProvider = serviceProvider;
        _cacheService = cacheService;
    }

    public async Task<List<User>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
            
        return await _cacheService.GetSetCachedItemAsync(typeof(User).ToCacheKey(request.Ids), async () =>
        {
            return await dbContext.Users.AsNoTracking().Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
        }) ?? new List<User>();
    }
}