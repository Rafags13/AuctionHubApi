using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.DTOs.User.Request.Create;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.DTOs.User.Request.Toggle;
using AuctionHub.Domain.DTOs.User.Response.RefreshToken;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> CreateAsync(RequestCreateUserDTO content, string hashedPassword, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> RefreshTokenAsync(RefreshTokenDTO content, long userId, CancellationToken cancellationToken = default);
        Task<ResponseRefreshTokenDTO?> GetRefreshInformationsAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<RequestGenerateTokenDTO?> GetUserByCredentialsAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default);
        Task<int> ToggleAsync(ToggleUserStatusDTO content, CancellationToken cancellationToken = default);
    }
}
