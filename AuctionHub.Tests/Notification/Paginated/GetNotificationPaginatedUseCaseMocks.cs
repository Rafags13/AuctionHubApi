using AuctionHub.Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Notification.Paginated
{
    public record GetNotificationPaginatedUseCaseMocks(
        Mock<IHttpContextAccessor> HttpContextAccessor,
        Mock<IUnitOfWork> UnitOfWork
    );
}
