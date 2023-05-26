using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogFodder.Core.Membership.Mapping
{
    public class UserDbMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.ExtendedData).ToJsonConversion(3500);
            builder.Property(x => x.PasswordHash).HasMaxLength(300);
            builder.Property(x => x.SecurityStamp).HasMaxLength(3000);
            builder.Property(x => x.ConcurrencyStamp).HasMaxLength(3000);
            builder.Property(x => x.PhoneNumber).HasMaxLength(100);
            builder.Property(x => x.UserName).HasMaxLength(150);
            builder.HasOne(x => x.ProfileImage);

            builder.HasMany(e => e.UserRoles)
               .WithOne(e => e.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();
        }
    }
}