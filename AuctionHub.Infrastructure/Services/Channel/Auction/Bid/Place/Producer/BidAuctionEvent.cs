using AuctionHub.Domain.DTOs.Auction.Bid.Request;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Producer
{
    public record BidAuctionEvent(long AuctionId, decimal Amount, long BidderId) : BidRequestDTO(AuctionId, Amount, BidderId)
    {
        public BidAuctionEvent(BidRequestDTO content) : this(content.AuctionId, content.Amount, content.BidderId)
        {
        }
    }
}
