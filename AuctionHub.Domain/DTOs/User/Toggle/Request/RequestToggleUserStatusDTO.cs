using AuctionHub.Domain.Enums.User;

namespace AuctionHub.Domain.DTOs.User.Toggle.Request
{
    public record RequestToggleUserStatusDTO(long UserId, EUserStatus Status);
}
