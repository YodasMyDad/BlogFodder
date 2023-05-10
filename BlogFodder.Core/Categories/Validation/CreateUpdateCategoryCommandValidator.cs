using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Shared.Validation;

namespace BlogFodder.Core.Categories.Validation;

public class CreateUpdateCategoryCommandValidator : BaseFluentValidator<CreateUpdateCategoryCommand>
{
    public CreateUpdateCategoryCommandValidator()
    {
        RuleFor(model => model.Category)
            .SetValidator(model => new CategoryValidator(model));
    }
}