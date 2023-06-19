using BlogFodder.Core.Media.Models;
using MediatR;

namespace BlogFodder.Core.Media.Commands;

public class GetFilesCommand : IRequest<List<BlogFodderFile>>
{
    public List<Guid> Ids { get; set; } = new();
}