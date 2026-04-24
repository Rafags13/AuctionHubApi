namespace AuctionHub.Domain.DTOs.User.Response.RefreshToken
{
    public record ResponseRefreshTokenDTO(long UserId, DateTime ExpirationDateTime);
}
