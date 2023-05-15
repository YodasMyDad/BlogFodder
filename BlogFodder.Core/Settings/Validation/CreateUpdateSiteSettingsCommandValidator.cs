using BlogFodder.Core.Settings.Commands;
using BlogFodder.Core.Shared.Validation;

namespace BlogFodder.Core.Settings.Validation;

public class CreateUpdateSiteSettingsCommandValidator : BaseFluentValidator<CreateUpdateSiteSettingsCommand>
{
    public CreateUpdateSiteSettingsCommandValidator()
    {
        RuleFor(model => model.SiteSettings)
            .SetValidator(model => new SiteSettingsValidator(model));
    }
}