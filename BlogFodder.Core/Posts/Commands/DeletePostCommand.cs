using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Posts.Commands;

public class DeletePostCommand : IRequest<HandlerResult<Post>>
{
    public Guid PostId { get; set; }
}