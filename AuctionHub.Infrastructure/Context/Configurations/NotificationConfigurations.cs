using AuctionHub.Domain.Constants.Notification;
using AuctionHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionHub.Infrastructure.Context.Configurations
{
    internal sealed class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(n => n.Message)
                .HasMaxLength(NotificationConstants.MESSAGE_LENGTH);
        }
    }
}
