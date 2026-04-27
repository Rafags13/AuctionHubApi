using AuctionHub.Domain.Enums.Notification;

namespace AuctionHub.Domain.DTOs.Notification.Read.Response
{
    public record ReadNotificationResponseDTO(ENotificationType Type, string Message, DateTime? ReadAt, DateTime? CreatedAt);
}
