using AuctionHub.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuctionHub.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IUserRepository UserRepository { get; }
        IAuctionRepository AuctionRepository { get; }
        IBidRepository BidRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        INotificationRepository NotificationRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
        Task<int> CommitAsync(CancellationToken ct);
    }
}
