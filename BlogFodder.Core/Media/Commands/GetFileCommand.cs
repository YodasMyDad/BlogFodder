using BlogFodder.Core.Media.Models;
using MediatR;

namespace BlogFodder.Core.Media.Commands;

public class GetFileCommand : IRequest<BlogFodderFile>
{
    public Guid? Id { get; set; }
}