using AuctionHub.Domain.DTOs.User.Request.Create;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class UserRepository(AuctionHubContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<bool> CreateAsync(RequestCreateUserDTO content, string hashedPassword, CancellationToken cancellationToken = default)
        {
            var user = User.Create(content, hashedPassword);

            await context.Users.AddAsync(user, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public Task<RequestGenerateTokenDTO?> GetUserByCredentialsAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default)
        {
            return context.Users.Where(u => u.Email == content.Email && u.PasswordHash == content.Password)
                .Select(u => new RequestGenerateTokenDTO(u.Id, u.Name, u.Role))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
