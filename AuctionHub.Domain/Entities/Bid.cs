using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.Enums.Auction;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHub.Domain.Entities
{
    public class Bid : BaseEntity
    {
        public decimal Amount { get; init; }
        public EBidStatus Status { get; private set; }
        public long AuctionId { get; init; }
        public long BidderId { get; init; }

        protected Bid() { }

        public Bid(BidRequestDTO content)
        {
            Amount = content.Amount;
            AuctionId = content.AuctionId;
            BidderId = content.BidderId;
            Status = EBidStatus.VALID;
        }

        #region [Foreign Key]
        [ForeignKey(nameof(AuctionId))]
        public Auction Auction { get; init; } = null!;

        [ForeignKey(nameof(BidderId))]
        public User Bidder { get; init; } = null!;
        #endregion

        #region [Factory]
        public static Bid Create(BidRequestDTO content)
        {
            return new Bid(content);
        }

        public void Outbid()
        {
            Status = EBidStatus.OUTBID;
        }
        #endregion
    }
}
