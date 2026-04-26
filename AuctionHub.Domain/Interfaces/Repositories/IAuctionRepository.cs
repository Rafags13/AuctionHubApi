using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IAuctionRepository : IBaseRepository<Auction>
    {
        Task<bool> CreateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default);
    }
}
