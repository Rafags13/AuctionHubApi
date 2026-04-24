using AuctionHub.Domain.Enums.User;

namespace AuctionHub.Domain.DTOs.User.Request.Toggle
{
    public record ToggleUserStatusDTO(long UserId, EUserStatus Status);
}
