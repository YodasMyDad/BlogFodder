using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Core.Posts.Commands;

public class CreateUpdatePostCommand : IRequest<HandlerResult<Post>>
{
    public IBrowserFile? FeaturedImage { get; set; } 
    public IBrowserFile? SocialImage { get; set; }
    public Post Post { get; set; } = new();
}