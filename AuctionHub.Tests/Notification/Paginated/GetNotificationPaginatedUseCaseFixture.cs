using AuctionHub.Application.UseCases.Notification.Queries;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Notification.Queries;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Notification.Paginated
{
    public class GetNotificationPaginatedUseCaseFixture
    {
        public IGetNotificationPaginatedUseCase Create(GetNotificationPaginatedUseCaseMocks? mocks = null)
        {
            var httpContextAccessor = mocks?.HttpContextAccessor ?? new Mock<IHttpContextAccessor>();
            var unitOfWork = mocks?.UnitOfWork ?? new Mock<IUnitOfWork>();

            return new GetNotificationPaginatedUseCase(
                httpContextAccessor.Object,
                unitOfWork.Object
            );
        }

        public PaginatedNotificationRequestDTO CreateRequest()
        => new(1, 10);
    }
}
