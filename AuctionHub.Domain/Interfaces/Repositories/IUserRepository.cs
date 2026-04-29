using AuctionHub.Domain.DTOs.Authentication.RefreshToken.Response;
using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.DTOs.User.Profile.Response;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.DTOs.User.Toggle.Request;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Enums.User;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> CreateAsync(RequestRegisterUserDTO content, string hashedPassword, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> RefreshTokenAsync(RefreshTokenDTO content, long userId, CancellationToken cancellationToken = default);
        Task<ResponseRefreshTokenDTO?> GetRefreshInformationsAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<RequestGenerateTokenDTO?> GetUserByCredentialsAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default);
        Task<int> ToggleAsync(RequestToggleUserStatusDTO content, CancellationToken cancellationToken = default);
        Task<ERole?> GetRoleAsync(long id, CancellationToken cancellationToken = default);
        Task<UserProfileDTO?> GetUserProfileAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> FindByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
