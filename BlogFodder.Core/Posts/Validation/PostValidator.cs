using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Shared.Validation;
using FluentValidation;

namespace BlogFodder.Core.Posts.Validation;

public class PostValidator : BaseFluentValidator<Post>
{
    public PostValidator(CreateUpdatePostCommand parent)
    {
        
        RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter a name");
        RuleFor(p => p.Excerpt).NotEmpty().WithMessage("You must enter an excerpt for this post");
        RuleFor(p => p.PageTitle).NotEmpty().WithMessage("The page title must not be empty");

        // TODO - This is not working?
        RuleFor(x => x.ContentItems)
            .Must(x => x != null && x.Any())
            .WithMessage("You need to add content to the blog");
    }
}