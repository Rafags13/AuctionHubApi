using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuctionHub.Infrastructure.UoW
{
    internal sealed class UnitOfWork(AuctionHubContext context) : IUnitOfWork
    {
        public IUserRepository UserRepository { get; private set; } = new UserRepository(context);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct)
        {
            return context.Database.BeginTransactionAsync(ct);
        }

        public Task<int> CommitAsync(CancellationToken ct)
        {
            return context.SaveChangesAsync(ct);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsyncCore()
        {
            await context.DisposeAsync();
        }
    }
}
