using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionHub.Infrastructure.Context.Configurations
{
    internal sealed class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasData(new User(1, "Admin", "admin@gmail.com", "VTy80B1UufqFBbxLspeoQqM6wTwuNvspE6uYszXi/4s=", ERole.ADMIN, EUserStatus.ACTIVE, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }
    }
}
