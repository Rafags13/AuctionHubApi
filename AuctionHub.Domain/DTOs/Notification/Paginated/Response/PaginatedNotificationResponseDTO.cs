using AuctionHub.Domain.Enums.Notification;

namespace AuctionHub.Domain.DTOs.Notification.Paginated.Response
{
    public record PaginatedNotificationResponseDTO(long Id, ENotificationType Type, DateTime? ReadAt);
}
