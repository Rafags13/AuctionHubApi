using System.Text.Json.Serialization;

namespace AuctionHub.Domain.DTOs.Auction.Create.Request
{
    public record RequestCreateAuctionDTO
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public decimal StartingPrice { get; init; }
        public DateTime StartTime { get; init; }

        [JsonIgnore]
        public long SellerId { get; private set; }

        public void SetSellerId(long sellerId)
        {
            SellerId = sellerId;
        }

        [JsonConstructor]
        protected RequestCreateAuctionDTO(string title, string description, decimal startingPrice, DateTime startTime)
        {
            Title = title;
            Description = description;
            StartingPrice = startingPrice;
            StartTime = startTime;
        }

        public RequestCreateAuctionDTO(string title, string description, decimal startingPrice, DateTime startTime, long sellerId) : this(title, description, startingPrice, startTime)
        {
            SellerId = sellerId;
        }
    }
}
