namespace AuctionHub.Domain.DTOs.Payment.Create.Request
{
    public record CreatePaymentRequestDTO(long AuctionId, long PayerId, decimal Amount);
}
