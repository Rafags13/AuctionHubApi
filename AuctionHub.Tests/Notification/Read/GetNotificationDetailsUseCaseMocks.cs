using AuctionHub.Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Notification.Read
{
    public record GetNotificationDetailsUseCaseMocks(
        Mock<IUnitOfWork> UnitOfWork,
        Mock<IHttpContextAccessor> HttpContextAccessor
    );
}
