using AuctionHub.Domain.DTOs.Auction.Bid.Response;
using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.DTOs.Auction.Details.Response;
using AuctionHub.Domain.DTOs.Auction.Ending.Response;
using AuctionHub.Domain.DTOs.Auction.Open.Response;
using AuctionHub.Domain.DTOs.Auction.UpdatePrice;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IAuctionRepository : IBaseRepository<Auction>
    {
        Task<bool> CreateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default);
        Task<EndingAuctionResponseDTO[]> GetExpiredAuctionsAsync(DateTime endedAt, CancellationToken cancellationToken = default);
        Task<bool> EndAsync(EndingAuctionResponseDTO content, CancellationToken cancellationToken = default);
        Task<bool> OpenAsync(OpenAuctionResponseDTO content, CancellationToken cancellationToken = default);
        Task<OpenAuctionResponseDTO[]> GetScheduledAuctionsToStartAsync(DateTime currentDateTime, CancellationToken cancellationToken = default);
        Task<bool> UpdateCurrentPriceAsync(RequestUpdateAuctionCurrentPriceDTO content, CancellationToken cancellationToken = default);
        Task<AuctionBidInformationsDTO?> GetAuctionBidInformationsAsync(long id, CancellationToken cancellationToken = default);
        Task<AuctionDetailsResponseDTO?> GetAsync(long id, CancellationToken cancellationToken = default);
        Task<AuctionNotificationInformationsDTO?> GetOpenAsync(long id, CancellationToken cancellationToken = default);
        Task<AuctionNotificationInformationsDTO?> GetOutBidAsync(long id, CancellationToken cancellationToken = default);
        Task<AuctionNotificationInformationsDTO?> GetWinnerAsync(long id, CancellationToken cancellationToken = default);
    }
}
