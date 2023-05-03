using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Shared.Validation;
using FluentValidation;

namespace BlogFodder.Core.Posts.Validation;

public class PostValidator : BaseFluentValidator<Post>
{
    public PostValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter a name");
        RuleFor(p => p.Excerpt).NotEmpty().WithMessage("You must enter an excerpt for this post");
        RuleFor(p => p.PageTitle).NotEmpty().WithMessage("The page title must not be empty");
    }
}