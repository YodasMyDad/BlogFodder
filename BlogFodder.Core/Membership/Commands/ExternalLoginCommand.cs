using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogFodder.Core.Membership.Commands
{
    public class ExternalLoginCommand : IRequest<AuthenticationResult>
    {
        public string? ProviderDisplayName { get; set; }

        public ExternalLoginInfo? ExternalLoginInfo { get;set; }

        public string? ReturnUrl { get; set; }
    }
}