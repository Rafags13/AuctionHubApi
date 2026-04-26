using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Auction.Bid.Commands
{
    public interface ICreateBidUseCase
    {
        Task<OneOf<bool, BaseError>> BidAsync(BidRequestDTO content, CancellationToken cancellationToken = default);
    }
}
