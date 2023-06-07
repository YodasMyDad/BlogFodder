using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Categories.Handlers;

public class GetCategoryHandler : IRequestHandler<GetCategoryCommand, Category?>
{
    private readonly IServiceProvider _serviceProvider;

    public GetCategoryHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Category?> Handle(GetCategoryCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        
        var category = dbContext.Categories.AsQueryable();

        if (request.IncludeSocialImage)
        {
            category = category.Include(x => x.SocialImage);
        }

        if (request.AsNoTracking)
        {
            category = category.AsNoTracking();
        }

        if (request.Id != null)
        {
            category = category.Where(x => x.Id == request.Id);
        }

        if (!request.Url.IsNullOrWhiteSpace())
        {
            category = category.Where(x => x.Url == request.Url);
        }

        return await category.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}