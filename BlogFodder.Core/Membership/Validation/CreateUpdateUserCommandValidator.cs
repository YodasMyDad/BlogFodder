using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using FluentValidation;

namespace BlogFodder.Core.Membership.Validation
{
    public class CreateUpdateUserCommandValidator : AbstractValidator<CreateUpdateUserCommand>
    {
        public CreateUpdateUserCommandValidator()
        {
            
            RuleFor(p => p.NewPasswordConfirmation).NotEmpty().When(p => !p.NewPassword.IsNullOrWhiteSpace());
            RuleFor(p => p.NewPasswordConfirmation).Equal(p => p.NewPassword).When(p => !p.NewPassword.IsNullOrWhiteSpace());
            
            RuleFor(model => model.User)
                .SetValidator(model => new UserValidation(model));
            
        }
    }
}