using AuctionHub.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuctionHub.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IUserRepository UserRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
        Task<int> CommitAsync(CancellationToken ct);
    }
}
