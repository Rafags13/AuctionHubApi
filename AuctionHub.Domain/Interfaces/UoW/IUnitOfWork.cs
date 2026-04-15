using Microsoft.EntityFrameworkCore.Storage;

namespace AuctionHub.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
        Task<int> CommitAsync(CancellationToken ct);
    }
}
