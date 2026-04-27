using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.DTOs.Notification.Paginated.Response;
using AuctionHub.Domain.Helpers.Autentication;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Notification.Queries;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Application.UseCases.Notification.Queries
{
    internal sealed class GetNotificationPaginatedUseCase(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork
    ) : IGetNotificationPaginatedUseCase
    {
        public Task<PaginatedDTO<PaginatedNotificationResponseDTO>> GetPaginatedAsync(PaginatedNotificationRequestDTO content, CancellationToken cancellationToken = default)
        {
            var userId = SessionHelper.GetUserId(httpContextAccessor.HttpContext);

            if (!userId.HasValue) return Task.FromResult(new PaginatedDTO<PaginatedNotificationResponseDTO>([], 0, 0, 0));

            return unitOfWork.NotificationRepository.GetPaginatedAsync(content, cancellationToken);
        }
    }
}
