using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Posts.Handlers;

public class GetPostsHandler : IRequestHandler<GetPostsCommand, PaginatedList<Post>>
{
    private readonly BlogFodderDbContext _context;
    public GetPostsHandler(BlogFodderDbContext context)
    {
        _context = context;
    }
    
    public Task<PaginatedList<Post>> Handle(GetPostsCommand request, CancellationToken cancellationToken)
    {
        var query = _context.Posts.AsQueryable();

        if (request.IncludeCategories)
        {
            query = query.Include(x => x.Categories);
        }
        
        if (request.IncludeFeaturedImage)
        {
            query = query.Include(x => x.FeaturedImage);
        }
        
        if (request.AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (request.CategoryIds.Any())
        {
            query = query.Where(p => p.Categories.Any(c => request.CategoryIds.Contains(c.Id)));
        }

        if (!request.SearchTerm.IsNullOrWhiteSpace())
        {
            query = query.Where(x => x.Name != null && x.Name.ToLower().Contains(request.SearchTerm.ToLower()));
        }

        query = request.OrderBy switch
        {
            GetPostsOrderBy.DateUpdated => query.OrderBy(p => p.DateUpdated),
            GetPostsOrderBy.DateUpdatedDescending => query.OrderByDescending(p => p.DateUpdated),
            GetPostsOrderBy.DateCreated => query.OrderBy(p => p.DateCreated),
            GetPostsOrderBy.DateCreatedDescending => query.OrderByDescending(p => p.DateCreated),
            _ => query.OrderByDescending(p => p.DateUpdated)
        };

        /*Posts = DbContext.Posts
            .AsQueryable()
            .Include(x => x.FeaturedImage)
            .Include(x => x.Categories)
            .AsNoTracking()
            .OrderByDescending(x => x.DateUpdated)
            .ToPaginatedList(CurrentPage, Settings.HomeAmountPerPage);*/

        return Task.FromResult(query.ToPaginatedList(request.PageIndex, request.AmountPerPage));
    }
}