using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.DTOs.User.Request.Login;

namespace AuctionHub.Domain.Interfaces.Services.Authentication.Login
{
    public interface IGenerateTokenService
    {
        string GenerateToken(RequestGenerateTokenDTO content);
        RefreshTokenDTO GenerateRefreshToken();
    }
}
