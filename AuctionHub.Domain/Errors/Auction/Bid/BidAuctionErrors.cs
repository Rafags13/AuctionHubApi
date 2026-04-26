using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Auction.Bid
{
    public record AuctionNotOpenedError() : BaseError("You can only bid on an opened auction.", nameof(AuctionNotOpenedError), StatusCodes.Status400BadRequest);
    //short the name of record below. NOW.
    public record BidBelowStartingPriceError() :
        BaseError("The amount should be equal to or higher than the starting price.", nameof(BidBelowStartingPriceError), StatusCodes.Status400BadRequest);
    public record AmountShouldBeHigherThenLastBidError() : BaseError("The amount should be higher than the last bid.", nameof(AmountShouldBeHigherThenLastBidError), StatusCodes.Status400BadRequest);
    public record CurrentUserIsntABidderError() : BaseError("The current user isn't a bidder.", nameof(CurrentUserIsntABidderError), StatusCodes.Status403Forbidden);
}
