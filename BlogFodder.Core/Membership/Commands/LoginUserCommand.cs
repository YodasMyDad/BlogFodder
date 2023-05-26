using System.ComponentModel.DataAnnotations;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace BlogFodder.Core.Membership.Commands
{
    public class LoginUserCommand : IRequest<AuthenticationResult>
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        public List<AuthenticationScheme> ExternalLogins { get; set; } = new();
    }
}