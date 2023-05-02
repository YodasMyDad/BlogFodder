using BlogFodder.Core.Posts.Models;
using FluentValidation;

namespace BlogFodder.Core.Posts.Validation;

public class PostContentItemValidator : AbstractValidator<PostContentItem>
{
    public PostContentItemValidator()
    {
        RuleFor(p => p.PluginAlias).NotEmpty().WithMessage("Unable to determine a plugin alias");
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<PostContentItem>.CreateWithOptions((PostContentItem)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}