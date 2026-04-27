namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Award.Producer
{
    public record AwardBidAuctionEvent(long AuctionId, decimal Amount);
}
