using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Shared.Validation;
using FluentValidation;

namespace BlogFodder.Core.Categories.Validation;

public class CategoryValidator : BaseFluentValidator<Category>
{
    public CategoryValidator(CreateUpdateCategoryCommand parent)
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter a name");
        RuleFor(p => p.PageTitle).NotEmpty().WithMessage("The page title must not be empty");
    }
}