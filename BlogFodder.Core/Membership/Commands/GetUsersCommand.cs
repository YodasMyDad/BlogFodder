using BlogFodder.Core.Membership.Models;
using MediatR;

namespace BlogFodder.Core.Membership.Commands;

public class GetUsersCommand : IRequest<List<User>>
{
    public List<Guid> Ids { get; set; } = new();
}