using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.DTOs.Notification.Paginated.Response;

namespace AuctionHub.Domain.Interfaces.UseCases.Notification.Queries
{
    public interface IGetNotificationPaginatedUseCase
    {
        Task<PaginatedDTO<PaginatedNotificationResponseDTO>> GetPaginatedAsync(PaginatedNotificationRequestDTO content, CancellationToken cancellationToken = default);
    }
}
