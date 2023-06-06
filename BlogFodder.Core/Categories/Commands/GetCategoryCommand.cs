using BlogFodder.Core.Categories.Models;
using MediatR;

namespace BlogFodder.Core.Categories.Commands;

public class GetCategoryCommand : IRequest<Category?>
{
    public Guid? Id { get; set; }
    public string? Url { get; set; }
    public bool IncludeSocialImage { get; set; } = true;
    public bool AsNoTracking { get; set; } = true;
}