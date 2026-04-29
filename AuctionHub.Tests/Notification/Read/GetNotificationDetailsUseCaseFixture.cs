using AuctionHub.Application.UseCases.Notification.Queries;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Notification.Queries;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Notification.Read
{
    public class GetNotificationDetailsUseCaseFixture
    {
        public IGetNotificationDetailsUseCase Create(GetNotificationDetailsUseCaseMocks? mocks = null)
        {
            var unitOfWork = mocks?.UnitOfWork ?? new Mock<IUnitOfWork>();
            var httpContextAccessor = mocks?.HttpContextAccessor ?? new Mock<IHttpContextAccessor>();

            return new GetNotificationDetailsUseCase(
                unitOfWork.Object,
                httpContextAccessor.Object
            );
        }
    }
}
