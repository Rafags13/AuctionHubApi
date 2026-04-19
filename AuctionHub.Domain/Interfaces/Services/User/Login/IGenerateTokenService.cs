using AuctionHub.Domain.DTOs.User.Request.Login;

namespace AuctionHub.Domain.Interfaces.Services.User.Login
{
    public interface IGenerateTokenService
    {
        string GenerateToken(RequestGenerateTokenDTO content);
    }
}
