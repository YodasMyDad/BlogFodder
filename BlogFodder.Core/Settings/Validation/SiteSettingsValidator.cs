using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Settings.Models;
using BlogFodder.Core.Shared.Validation;
using FluentValidation;

namespace BlogFodder.Core.Settings.Validation;

public class SiteSettingsValidator : BaseFluentValidator<SiteSettings>
{
    public SiteSettingsValidator(CreateUpdateSiteSettingsCommand parent)
    {
        RuleFor(p => p.SiteName).NotEmpty().WithMessage("You must enter a site name");
        RuleFor(p => p.DefaultPageTitle).NotEmpty().WithMessage("You must enter a page title");
        RuleFor(p => p.DefaultMetaDescription).NotEmpty().WithMessage("You must enter a meta description");
    }
}