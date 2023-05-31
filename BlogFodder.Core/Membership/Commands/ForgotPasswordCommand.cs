using System.ComponentModel.DataAnnotations;
using BlogFodder.Core.Membership.Models;
using MediatR;

namespace BlogFodder.Core.Membership.Commands;

public class ForgotPasswordCommand : IRequest<AuthenticationResult>
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}