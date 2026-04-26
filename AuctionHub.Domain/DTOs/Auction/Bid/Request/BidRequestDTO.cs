using System.Text.Json.Serialization;

namespace AuctionHub.Domain.DTOs.Auction.Bid.Request
{
    public record BidRequestDTO
    {
        public long AuctionId { get; init; }

        public decimal Amount { get; init; }

        [JsonIgnore]
        public long? OutBidId { get; private set; }

        [JsonIgnore]
        public long BidderId { get; private set; }

        [JsonConstructor]
        public BidRequestDTO(long auctionId, decimal amount)
        {
            AuctionId = auctionId;
            Amount = amount;
        }

        public BidRequestDTO(long auctionId, decimal amount, long? outBidId, long bidderId) : this(auctionId, amount) {

            SetOutBidId(outBidId);
            SetBidderId(bidderId);
        }

        public void SetBidderId(long bidderId)
        {
            BidderId = bidderId;
        }

        public void SetOutBidId(long? outBidId)
        {
            OutBidId = outBidId;
        }
    }
}
