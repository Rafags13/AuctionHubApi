using AuctionHub.Domain.Enums.Notification;
using AuctionHub.Domain.Helpers.Notification;

namespace AuctionHub.Domain.DTOs.Notification.Create
{
    public record CreateOutBidNotificationRequestDTO(string AuctionTitle, long UserId) :
        CreateNotificationRequestDTO(ENotificationType.OUTBID, GenerateLayoutMessageHelper.GenerateOutbidMessage(AuctionTitle), UserId);

    public record CreateStartAuctionNotificationRequestDTO(string AuctionTitle, long UserId) :
        CreateNotificationRequestDTO(ENotificationType.AUCTION_STARTED, GenerateLayoutMessageHelper.GenerateAuctionStartedMessage(AuctionTitle), UserId);

    public record CreateWonAuctionNotificationRequestDTO(string AuctionTitle, long UserId) :
        CreateNotificationRequestDTO(ENotificationType.WIN, GenerateLayoutMessageHelper.GenerateAuctionWonMessage(AuctionTitle), UserId);

    public record CreateNotificationRequestDTO(ENotificationType Type, string Message, long UserId);
}
