using AuctionHub.Domain.Interfaces.Services.External.Payment;

namespace AuctionHub.Infrastructure.Services.External.Payment
{
    internal sealed class ExternalIntegrationPaymentService : IExternalIntegrationPaymentService
    {
        private readonly short DELAY_IN_SECONDS = 2 * 1000;

        public async Task<bool> ProcessAsync(long Id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(DELAY_IN_SECONDS, cancellationToken);

            //TODO: In a real implementation, this method would call an external payment gateway API to process the payment and return the result.
            return Id % 2 == 0;
        }
    }
}
