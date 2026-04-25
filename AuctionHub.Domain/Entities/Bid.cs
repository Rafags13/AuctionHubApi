using AuctionHub.Domain.Enums.Auction;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHub.Domain.Entities
{
    public class Bid : BaseEntity
    {
        public decimal Amount { get; init; }
        public EBidStatus Status { get; init; }
        public long AuctionId { get; init; }
        public long BidderId { get; init; }

        #region [Foreign Key]
        [ForeignKey(nameof(AuctionId))]
        public Auction Auction { get; init; } = null!;

        [ForeignKey(nameof(BidderId))]
        public User Bidder { get; init; } = null!;
        #endregion
    }
}
