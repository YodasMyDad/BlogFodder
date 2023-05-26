using Microsoft.AspNetCore.Identity;

namespace BlogFodder.Core.Membership.Models;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public Dictionary<string, object> ExtendedData { get; set; } = new();
    public List<UserRole> UserRoles { get; set; } = new();
}