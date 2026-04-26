using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.DTOs.Auction.Ending.Response;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IAuctionRepository : IBaseRepository<Auction>
    {
        Task<bool> CreateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default);
        Task<EndingAuctionResponseDTO[]> GetExpiredAuctionsAsync(DateTime endedAt, CancellationToken cancellationToken = default);
        Task<bool> EndAsync(EndingAuctionResponseDTO content, CancellationToken cancellationToken = default);
    }
}
