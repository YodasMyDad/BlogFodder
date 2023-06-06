using System.Security.Claims;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlogFodder.Core.Membership.Claims
{
    public class BlogFodderUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly BlogFodderDbContext _context;

        public BlogFodderUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor, BlogFodderDbContext context)
                : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
            _context = context;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var dbUser = await _context.Users.Include(x => x.ProfileImage).AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id);
            user = dbUser!;
            
            var principal = await base.CreateAsync(user).ConfigureAwait(false);
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var claimsToAdd = new List<Claim> {new(Constants.Claims.Md5Hash, user.Email?.ToMd5() ?? string.Empty)};

            if (user.ProfileImage?.Url.IsNullOrWhiteSpace() == false)
            {
                claimsToAdd.Add(new Claim(Constants.Claims.ProfileImage, user.ProfileImage.Url));
            }

            if (roles.Count > 0)
            {
                foreach (var r in roles)
                {
                    claimsToAdd.Add(new Claim(ClaimTypes.Role, r));
                }
            }

            if (claimsToAdd.Count > 0)
            {
                ((ClaimsIdentity)principal.Identity!).AddClaims(claimsToAdd.ToArray());
            }

            return principal;
        }
    }
}