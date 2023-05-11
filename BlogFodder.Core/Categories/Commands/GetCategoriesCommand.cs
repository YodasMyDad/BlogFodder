using BlogFodder.Core.Categories.Models;
using MediatR;

namespace BlogFodder.Core.Categories.Commands;

public class GetCategoriesCommand : IRequest<List<Category>>
{
    
}