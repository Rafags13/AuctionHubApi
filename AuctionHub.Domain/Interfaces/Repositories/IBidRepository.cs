using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.DTOs.Auction.Bid.Response;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IBidRepository : IBaseRepository<Bid>
    {
        Task<bool> CreateAndOutBidAsync(BidRequestDTO content, CancellationToken cancellationToken = default);
        Task<BidInformationsResponseDTO?> GetBidToOutBidAsync(long auctionId, CancellationToken cancellationToken = default);
    }
}
