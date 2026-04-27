using AuctionHub.Domain.DTOs.Payment.Create.Request;
using AuctionHub.Domain.DTOs.Payment.Pay.Request;
using AuctionHub.Domain.Entities;

namespace AuctionHub.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<long?> CreateAsync(CreatePaymentRequestDTO content, CancellationToken cancellationToken = default);
        Task<bool> PayAsync(PayPaymentRequestDTO content, CancellationToken cancellationToken = default);
        Task<bool> FailAsync(long id, CancellationToken cancellationToken = default);
    }
}
