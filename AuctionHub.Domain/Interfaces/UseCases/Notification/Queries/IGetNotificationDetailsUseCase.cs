using AuctionHub.Domain.DTOs.Notification.Read.Response;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Notification.Queries
{
    public interface IGetNotificationDetailsUseCase
    {
        Task<OneOf<ReadNotificationResponseDTO, BaseError>> GetAsync(long id, CancellationToken cancellationToken = default);
    }
}
