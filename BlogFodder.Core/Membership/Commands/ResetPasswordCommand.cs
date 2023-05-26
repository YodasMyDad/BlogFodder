using System.ComponentModel.DataAnnotations;
using BlogFodder.Core.Membership.Models;
using MediatR;

namespace BlogFodder.Core.Membership.Commands
{
    public class ResetPasswordCommand : IRequest<AuthenticationResult>
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = Constants.Identity.MinimumPasswordLength)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}