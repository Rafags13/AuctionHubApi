using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Common.Auction
{
    public record AuctionNotFoundError() : BaseError("Auction not found.", nameof(AuctionNotFoundError), StatusCodes.Status404NotFound);
}
