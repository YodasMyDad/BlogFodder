using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Shared.Validation;
using FluentValidation;

namespace BlogFodder.Core.Posts.Validation;

public class CreateUpdatePostCommandValidator : BaseFluentValidator<CreateUpdatePostCommand>
{
    public CreateUpdatePostCommandValidator()
    {
        //RuleFor(p => p.Post);
        RuleFor(p => p.FeaturedImage).NotNull().WithMessage("You must select a featured image");
        RuleFor(model => model.Post)
            .SetValidator(model => new PostValidator(model));
    }
}