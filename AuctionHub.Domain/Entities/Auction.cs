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

        #region [Foreign Key]
        [ForeignKey(nameof(SellerId))]
        public User Seller { get; init; } = null!;

        [ForeignKey(nameof(WinnerId))]
        public User? Winner { get; init; }
        #endregion
    }
}
