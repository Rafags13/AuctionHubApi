using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IBidRepository : IBaseRepository<Bid>
    {
        Task<long?> CreateAsync(BidRequestDTO content, CancellationToken cancellationToken = default);
        Task<bool> OutbidAsync(long outBidId, CancellationToken cancellationToken = default);
        Task<bool> CancelAsync(long id, CancellationToken cancellationToken = default);
        Task<long?> GetOutBidIdAsync(long auctionId, CancellationToken cancellationToken = default);
        Task<decimal?> GetLastBidAmountAsync(long auctionId, CancellationToken cancellationToken = default);
    }
}
