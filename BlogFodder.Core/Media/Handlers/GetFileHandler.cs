using BlogFodder.Core.Data;
using BlogFodder.Core.Media.Commands;
using BlogFodder.Core.Media.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Media.Handlers;

public class GetFileHandler : IRequestHandler<GetFileCommand, BlogFodderFile?>
{
    private readonly IServiceProvider _serviceProvider;
    public GetFileHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task<BlogFodderFile?> Handle(GetFileCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        var query = dbContext.Files.AsQueryable();
        return await query.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, 
                cancellationToken: cancellationToken);
    }
}