using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.DTOs.Notification.Read.Request;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class NotificationRepository(AuctionHubContext context) : BaseRepository<Notification>(context), INotificationRepository
    {
        public async Task<bool> CreateAsync(CreateNotificationRequestDTO content, CancellationToken cancellationToken = default)
        {
            var notification = Notification.Create(content);

            await context.AddAsync(notification, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> ReadAsync(ReadNotificationRequestDTO content, CancellationToken cancellationToken = default)
        {
            var notification = await FirstOrDefaultAsync(n => n.Id == content.Id, cancellationToken);

            if (notification == null)
                return false;

            notification.Read(content.ReadAt);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
