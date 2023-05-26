using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace BlogFodder.Core.Membership.Commands
{
    public class CreateUpdateUserCommand : IRequest<HandlerResult<User>>
    {
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? NewPasswordConfirmation { get; set; }
        public User User { get; set; } = new();
        public IBrowserFile? ProfileImageUpload { get; set; }
    }
}