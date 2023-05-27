using BlogFodder.Core.Membership.Models;
using MediatR;

namespace BlogFodder.Core.Membership.Commands
{
    public class GetUserByIdCommand : IRequest<User?>
    {
        public Guid Id { get; set; }
    }
}