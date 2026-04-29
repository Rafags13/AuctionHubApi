using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.Enums.Notification;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHub.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public ENotificationType Type { get; init; }
        public string Message { get; init; } = string.Empty;
        public DateTime? ReadAt { get; private set; }
        public long UserId { get; init; }

        protected Notification() { }

        public Notification(CreateNotificationRequestDTO content)
        {
            Type = content.Type;
            Message = content.Message;
            UserId = content.UserId;
        }

        #region [Foreign Keys]
        [ForeignKey(nameof(UserId))]
        public User User { get; private set; } = null!;
        #endregion

        #region [Factory]
        public static Notification Create(CreateNotificationRequestDTO content)
        {
            return new Notification(content);
        }

        public void Read(DateTime readAt)
        {
            ReadAt = readAt;
        }
        #endregion
    }
}
