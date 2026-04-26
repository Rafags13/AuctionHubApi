using System.Text.Json.Serialization;

namespace AuctionHub.Domain.DTOs.Auction.Create.Request
{
    public record RequestCreateAuctionDTO
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public decimal StartingPrice { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }

        [JsonIgnore]
        public long SellerId { get; private set; }

        public void SetSellerId(long sellerId)
        {
            SellerId = sellerId;
        }

        [JsonConstructor]
        protected RequestCreateAuctionDTO(string title, string description, decimal startingPrice, DateTime startTime, DateTime endTime)
        {
            Title = title;
            Description = description;
            StartingPrice = startingPrice;
            StartTime = startTime;
            EndTime = endTime;
        }

        public RequestCreateAuctionDTO(
            string title,
            string description,
            decimal startingPrice,
            DateTime startTime,
            DateTime endTime,
            long sellerId) : this(title, description, startingPrice, startTime, endTime)
        {
            SellerId = sellerId;
        }
    }
}
