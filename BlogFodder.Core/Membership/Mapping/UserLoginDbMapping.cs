using BlogFodder.Core.Membership.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Membership.Mapping
{
    public class UserLoginDbMapping : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.Property(x => x.ProviderDisplayName).HasMaxLength(3000);
        }
    }
}