using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.Enums.Auction;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHub.Domain.Entities
{
    public class Auction : BaseEntity
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal StartingPrice { get; init; }
        public decimal? CurrentPrice { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime? EndTime { get; init; }
        public EAuctionStatus Status { get; init; }
        public long SellerId { get; init; }
        public long? WinnerId { get; init; }

        protected Auction() { }

        public Auction(RequestCreateAuctionDTO content)
        {
            Title = content.Title;
            Description = content.Description;
            StartingPrice = content.StartingPrice;
            StartTime = content.StartTime;
            SellerId = content.SellerId;
            Status = EAuctionStatus.SCHEDULED;
        }

        #region [Foreign Key]
        [ForeignKey(nameof(SellerId))]
        public User Seller { get; init; } = null!;

        [ForeignKey(nameof(WinnerId))]
        public User? Winner { get; init; }
        #endregion

        #region [Factory]
        public static Auction Create(RequestCreateAuctionDTO content)
        {
            return new Auction(content);
        }
        #endregion
    }
}
