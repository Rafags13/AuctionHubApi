using AuctionHub.Domain.Enums.User;

namespace AuctionHub.Domain.DTOs.User.Request.Login
{
    public record RequestGenerateTokenDTO(long Id, string Name, ERole Role, EUserStatus Status);
}
