﻿using Microsoft.AspNetCore.Identity;

namespace BlogFodder.Core.Membership.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public Role Role { get; set; } = default!;
    public User User { get; set; } = default!;
}