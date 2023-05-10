using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Core.Categories.Commands;

public class CreateUpdateCategoryCommand : IRequest<HandlerResult<Category>>
{
    public IBrowserFile? SocialImage { get; set; }
    public Category Category { get; set; } = new();
    public bool IsUpdate { get; set; }
}