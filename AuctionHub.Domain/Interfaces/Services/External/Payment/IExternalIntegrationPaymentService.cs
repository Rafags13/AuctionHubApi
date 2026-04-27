namespace AuctionHub.Domain.Interfaces.Services.External.Payment
{
    public interface IExternalIntegrationPaymentService
    {
        Task<bool> ProcessAsync(long Id, CancellationToken cancellationToken = default);
    }
}
