using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.DTOs.Notification.Paginated.Response;
using AuctionHub.Domain.DTOs.Notification.Read.Request;
using AuctionHub.Domain.DTOs.Notification.Read.Response;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<bool> CreateAsync(CreateNotificationRequestDTO content, CancellationToken cancellationToken = default);
        Task<bool> ReadAsync(ReadNotificationRequestDTO content, CancellationToken cancellationToken = default);
        Task<ReadNotificationResponseDTO?> GetAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> IsAllowedToReadAsync(long id, long userId, CancellationToken cancellationToken = default);
        Task<PaginatedDTO<PaginatedNotificationResponseDTO>> GetPaginatedAsync(PaginatedNotificationRequestDTO content, CancellationToken cancellationToken = default);
    }
}
