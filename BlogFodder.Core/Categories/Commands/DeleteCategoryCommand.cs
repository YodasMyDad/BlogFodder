using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Categories.Commands;

public class DeleteCategoryCommand : IRequest<HandlerResult<Category>>
{
    public Guid CategoryId { get; set; }
}