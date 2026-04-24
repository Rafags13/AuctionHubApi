namespace AuctionHub.Domain.DTOs.User.Common
{
    public record RefreshTokenDTO(string RefreshToken, DateTime ExpiresAt);
}
