using BlogFodder.Core.Data;
using BlogFodder.Core.Media.Commands;
using BlogFodder.Core.Media.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Media.Handlers;

public class GetFilesHandler : IRequestHandler<GetFilesCommand, List<BlogFodderFile>>
{
    private readonly IServiceProvider _serviceProvider;
    public GetFilesHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<List<BlogFodderFile>> Handle(GetFilesCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        var query = dbContext.Files.AsQueryable();
        return await query.AsNoTracking()
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}