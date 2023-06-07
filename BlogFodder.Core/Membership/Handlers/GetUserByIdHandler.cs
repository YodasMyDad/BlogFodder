using BlogFodder.Core.Data;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Membership.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, User?>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetUserByIdHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<User?> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
            return await dbContext.FindAsync<User>(new object[] { request.Id }, cancellationToken: cancellationToken)
                                    .ConfigureAwait(false);
        }
    }
}