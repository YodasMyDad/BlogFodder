using BlogFodder.Core.Membership.Models;
using MediatR;

namespace BlogFodder.Core.Membership.Commands;

    public class ConfirmEmailCommand : IRequest<AuthenticationResult>
    {
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public bool IsEmailUpdate { get; set; }
    }
