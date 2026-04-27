using AuctionHub.Domain.DTOs.Auction.Details.Response;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Auction.Details.Queries
{
    public interface IGetAuctionInformationsUseCase
    {
        Task<OneOf<AuctionDetailsResponseDTO, BaseError>> GetAsync(long auctionId, CancellationToken cancellationToken = default);
    }
}
