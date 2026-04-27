using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.Enums.Notification;

namespace AuctionHub.Infrastructure.Services.Channel.Notification.Create.Producer
{
    public record CreateNotificationEvent(ENotificationType Type, string Message, long UserId) : CreateNotificationRequestDTO(Type, Message, UserId)
    {
        public CreateNotificationEvent(CreateNotificationRequestDTO content) : this(content.Type, content.Message, content.UserId) {}
    }
}
