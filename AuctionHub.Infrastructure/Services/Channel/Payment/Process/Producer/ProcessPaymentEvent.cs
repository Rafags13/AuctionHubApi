namespace AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer
{
    public record ProcessPaymentEvent(long Id, decimal Amount, long BidId, long AuctionId);
}
