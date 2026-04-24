using AuctionHub.Domain.DTOs.Authentication.RefreshToken.Response;
using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.DTOs.User.Toggle.Request;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class UserRepository(AuctionHubContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<bool> CreateAsync(RequestRegisterUserDTO content, string hashedPassword, CancellationToken cancellationToken = default)
        {
            var user = User.Create(content, hashedPassword);

            await context.Users.AddAsync(user, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public Task<ResponseRefreshTokenDTO?> GetRefreshInformationsAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return context.Users.Where(u => u.RefreshToken == refreshToken && u.ExpirationRefreshToken.HasValue)
                .Select(u => new ResponseRefreshTokenDTO(u.Id, u.ExpirationRefreshToken.Value))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<RequestGenerateTokenDTO?> GetUserByCredentialsAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default)
        {
            return context.Users.Where(u => u.Email == content.Email && u.PasswordHash == content.Password)
                .Select(u => new RequestGenerateTokenDTO(u.Id, u.Name, u.Role, u.Status))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> RefreshTokenAsync(RefreshTokenDTO content, long userId, CancellationToken cancellationToken = default)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                return false;

            user.Refresh(content);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task<int> ToggleAsync(RequestToggleUserStatusDTO content, CancellationToken cancellationToken = default)
        {
            return context.Users
                .Where(u => u.Id == content.UserId)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(p => p.Status, content.Status), cancellationToken);
        }
    }
}
