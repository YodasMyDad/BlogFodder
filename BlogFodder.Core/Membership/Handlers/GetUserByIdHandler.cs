using Gab.Core.Data.Context;
using Gab.Core.Membership.Models.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using BlogFodder.Core.Membership.Commands;

namespace Gab.Core.Membership.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, GabUser>
    {
        private readonly IDbContextFactory<GabDbContext> _dbFactory;

        public GetUserByIdHandler(IDbContextFactory<GabDbContext> db)
        {
            _dbFactory = db;
        }

        public async Task<GabUser> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.FindAsync<GabUser>(new object[] { request.Id }, cancellationToken: cancellationToken)
                                    .ConfigureAwait(false);
        }
    }
}