using AuctionHub.Domain.DTOs.Auction.Bid.Request;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Producer
{
    public record BidAuctionEvent(long AuctionId, decimal Amount, long? OutBidId, long BidderId) : BidRequestDTO(AuctionId, Amount, OutBidId, BidderId)
    {
        public BidAuctionEvent(BidRequestDTO content) : this(content.AuctionId, content.Amount, content.OutBidId, content.BidderId)
        {
        }
    }
}
