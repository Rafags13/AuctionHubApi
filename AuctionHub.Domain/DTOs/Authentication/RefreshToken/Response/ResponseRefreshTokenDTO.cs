namespace AuctionHub.Domain.DTOs.Authentication.RefreshToken.Response
{
    public record ResponseRefreshTokenDTO(long UserId, DateTime ExpirationDateTime);
}
