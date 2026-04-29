using AuctionHub.Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.User.Profile
{
    public record GetUserProfileUseCaseMocks(
        Mock<IHttpContextAccessor> HttpContextAccessor,
        Mock<IUnitOfWork> UnitOfWork
    );
}
