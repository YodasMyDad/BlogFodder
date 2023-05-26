using BlogFodder.Core.Membership.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Membership.Mapping
{
    public class UserClaimDbMapping : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.Property(x => x.ClaimValue).HasMaxLength(3000);
            builder.Property(x => x.ClaimType).HasMaxLength(3000);
        }
    }
}