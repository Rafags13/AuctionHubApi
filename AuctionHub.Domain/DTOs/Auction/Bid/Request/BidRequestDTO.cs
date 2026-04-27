using System.Text.Json.Serialization;

namespace AuctionHub.Domain.DTOs.Auction.Bid.Request
{
    public record BidRequestDTO
    {
        public long AuctionId { get; init; }

        public decimal Amount { get; init; }

        [JsonIgnore]
        public long BidderId { get; private set; }

        [JsonConstructor]
        public BidRequestDTO(long auctionId, decimal amount)
        {
            AuctionId = auctionId;
            Amount = amount;
        }

        public BidRequestDTO(long auctionId, decimal amount, long bidderId) : this(auctionId, amount) {
            SetBidderId(bidderId);
        }

        public void SetBidderId(long bidderId)
        {
            BidderId = bidderId;
        }
    }
}
