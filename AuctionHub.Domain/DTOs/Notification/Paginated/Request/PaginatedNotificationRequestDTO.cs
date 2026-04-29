using System.Text.Json.Serialization;

namespace AuctionHub.Domain.DTOs.Notification.Paginated.Request
{
    public record PaginatedNotificationRequestDTO(int Page, sbyte PageSize)
    {
        [JsonIgnore]
        public long UserId { get; private set; }

        public void SetUserId(long userId)
        {
            UserId = userId;
        }
    }
}
