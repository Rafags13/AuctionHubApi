using AuctionHub.Domain.DTOs.User.Request.Create;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> CreateAsync(RequestCreateUserDTO content, string hashedPassword, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
