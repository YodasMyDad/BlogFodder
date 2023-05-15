using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Shared.Validation;
using FluentValidation;

namespace BlogFodder.Core.Posts.Validation;

public class CreateUpdatePostCommandValidator : BaseFluentValidator<CreateUpdatePostCommand>
{
    public CreateUpdatePostCommandValidator()
    {
        RuleFor(model => model.Post)
            .SetValidator(model => new PostValidator(model));
    }
}