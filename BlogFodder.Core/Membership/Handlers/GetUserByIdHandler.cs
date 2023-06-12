using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Membership.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, User?>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICacheService _cacheService;

        public GetUserByIdHandler(IServiceProvider serviceProvider, ICacheService cacheService)
        {
            _serviceProvider = serviceProvider;
            _cacheService = cacheService;
        }

        public async Task<User?> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
            
            return await _cacheService.GetSetCachedItemAsync(typeof(User).ToCacheKey(request.Id.ToString()), async () =>
            {
                return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            });
        }
    }
}