using AuctionHub.Domain.DTOs.Payment.Create.Request;
using AuctionHub.Domain.DTOs.Payment.Pay.Request;
using AuctionHub.Domain.Enums.Payment;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHub.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; init; }
        public EPaymentStatus Status { get; private set; }
        public DateTime? PayedAt { get; private set; }
        public long AuctionId { get; init; }
        public long PayerId { get; init; }

        protected Payment() { }

        public Payment(CreatePaymentRequestDTO content)
        {
            Amount = content.Amount;
            AuctionId = content.AuctionId;
            PayerId = content.PayerId;
            Status = EPaymentStatus.PENDING;
        }

        #region [Foreign Keys]
        [ForeignKey(nameof(AuctionId))]
        public Auction Auction { get; private set; } = null!;

        [ForeignKey(nameof(PayerId))]
        public User Payer { get; private set; } = null!;
        #endregion

        #region [Factory]
        public static Payment Create(CreatePaymentRequestDTO content)
        {
            return new Payment(content);
        }

        public void Pay(PayPaymentRequestDTO content)
        {
            Status = EPaymentStatus.PAID;
            PayedAt = content.CurrentDate;
        }

        public void Fail() {
            Status = EPaymentStatus.FAILED;
        }
        #endregion
    }
}
