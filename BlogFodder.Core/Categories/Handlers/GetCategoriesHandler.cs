using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.Core.Categories.Handlers;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesCommand, List<Category>>
{
    private readonly BlogFodderDbContext _dbContext;

    public GetCategoriesHandler(BlogFodderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    public async Task<List<Category>> Handle(GetCategoriesCommand request, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.ToListAsync(cancellationToken: cancellationToken);
    }
}