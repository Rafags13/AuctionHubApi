using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.DTOs.Notification.Read.Request;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<bool> CreateAsync(CreateNotificationRequestDTO content, CancellationToken cancellationToken = default);
        Task<bool> ReadAsync(ReadNotificationRequestDTO content, CancellationToken cancellationToken = default);
    }
}
