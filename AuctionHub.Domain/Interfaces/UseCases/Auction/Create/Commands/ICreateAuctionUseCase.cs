using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Auction.Create.Commands
{
    public interface ICreateAuctionUseCase
    {
        Task<OneOf<bool, BaseError>> CreateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default);
    }
}
