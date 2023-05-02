using BlogFodder.Core.Posts.Models;
using FluentValidation;

namespace BlogFodder.Core.Posts.Validation;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter a name");
        RuleFor(p => p.Excerpt).NotEmpty().WithMessage("You must enter an excerpt for this post");
        RuleForEach(x => x.ContentItems)
            .SetValidator(new PostContentItemValidator());
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Post>.CreateWithOptions((Post)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}