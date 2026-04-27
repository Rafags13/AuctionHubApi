using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.DTOs.Auction.Ending.Response;
using AuctionHub.Domain.Enums.Auction;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHub.Domain.Entities
{
    public class Auction : BaseEntity
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal StartingPrice { get; init; }
        public decimal? CurrentPrice { get; private set; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public EAuctionStatus Status { get; private set; }
        public long SellerId { get; init; }
        public long? WinnerId { get; private set; }

        protected Auction() { }

        public Auction(RequestCreateAuctionDTO content)
        {
            Title = content.Title;
            Description = content.Description;
            StartingPrice = content.StartingPrice;
            StartTime = content.StartTime;
            EndTime = content.EndTime;
            SellerId = content.SellerId;
            Status = EAuctionStatus.SCHEDULED;
        }

        #region [Foreign Key]
        [ForeignKey(nameof(SellerId))]
        public User Seller { get; init; } = null!;

        [ForeignKey(nameof(WinnerId))]
        public User? Winner { get; init; }
        #endregion

        #region [Navigations]
        public ICollection<Bid> Bids { get; private set; } = [];
        #endregion

        #region [Factory]
        public static Auction Create(RequestCreateAuctionDTO content)
        {
            return new Auction(content);
        }

        public void UpdateCurrentPrice(decimal newPrice)
        {
            CurrentPrice = newPrice;
        }

        public void End(EndingAuctionResponseDTO content)
        {
            Status = EAuctionStatus.CLOSED;
            WinnerId = content.LastBidderId;
        }

        public void Open()
        {
            Status = EAuctionStatus.OPEN;
        }
        #endregion
    }
}
