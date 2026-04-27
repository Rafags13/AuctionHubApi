using AuctionHub.Domain.DTOs.Payment.Create.Request;
using AuctionHub.Domain.DTOs.Payment.Pay.Request;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class PaymentRepository(AuctionHubContext context) : BaseRepository<Payment>(context), IPaymentRepository
    {
        public async Task<long?> CreateAsync(CreatePaymentRequestDTO content, CancellationToken cancellationToken = default)
        {
            var payment = Payment.Create(content);

            await context.Payments.AddAsync(payment, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return payment?.Id;
        }

        public async Task<bool> FailAsync(long id, CancellationToken cancellationToken = default)
        {
            var payment = await FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (payment is null)
                return false;

            payment.Fail();

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> PayAsync(PayPaymentRequestDTO content, CancellationToken cancellationToken = default)
        {
            var payment = await FirstOrDefaultAsync(p => p.Id == content.Id, cancellationToken);

            if (payment is null)
                return false;

            payment.Pay(content);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
