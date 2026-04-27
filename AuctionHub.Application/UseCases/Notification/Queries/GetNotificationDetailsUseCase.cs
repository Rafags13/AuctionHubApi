using AuctionHub.Domain.DTOs.Notification.Read.Request;
using AuctionHub.Domain.DTOs.Notification.Read.Response;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Errors.Common.Notification;
using AuctionHub.Domain.Errors.Notification.Read;
using AuctionHub.Domain.Helpers.Autentication;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Notification.Queries;
using Microsoft.AspNetCore.Http;
using OneOf;

namespace AuctionHub.Application.UseCases.Notification.Queries
{
    internal sealed class GetNotificationDetailsUseCase(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor
    ) : IGetNotificationDetailsUseCase
    {
        public async Task<OneOf<ReadNotificationResponseDTO, BaseError>> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            var loggedUserId = SessionHelper.GetUserId(httpContextAccessor.HttpContext);
            if (!loggedUserId.HasValue || !await unitOfWork.NotificationRepository.IsAllowedToReadAsync(id, loggedUserId.Value, cancellationToken))
                return new NotificationDoesntBelongToCurrentUserError();

            var notification = await unitOfWork.NotificationRepository.GetAsync(id, cancellationToken);

            if (notification is null)
                return new NotificationNotFoundError();

            if (notification.ReadAt is null)
                await unitOfWork.NotificationRepository.ReadAsync(new ReadNotificationRequestDTO(notification.Id, DateTime.UtcNow), cancellationToken);

            return notification;
        }
    }
}
