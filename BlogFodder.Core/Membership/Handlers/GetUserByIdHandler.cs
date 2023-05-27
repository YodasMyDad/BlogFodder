using BlogFodder.Core.Data;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Membership.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, User?>
    {
        private readonly IDbContextFactory<BlogFodderDbContext> _dbFactory;

        public GetUserByIdHandler(IDbContextFactory<BlogFodderDbContext> db)
        {
            _dbFactory = db;
        }

        public async Task<User?> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await db.FindAsync<User>(new object[] { request.Id }, cancellationToken: cancellationToken)
                                    .ConfigureAwait(false);
        }
    }
}