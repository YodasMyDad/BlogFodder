using BlogFodder.Core.Membership.Models;
using MediatR;

namespace BlogFodder.Core.Email.Commands;

public class SendEmailConfirmationCommand : IRequest
{
    public User? User { get; set; }
    public string? NewEmailAddress { get; set; }
    public string ReturnUrl { get; set; } = "/";
}