using AuctionHub.Application.UseCases.User.Queries;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.User.Queries;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.User.Profile
{
    public class GetUserProfileUseCaseFixture
    {
        public IGetUserProfileUseCase Create(GetUserProfileUseCaseMocks? mocks = null)
        {
            var httpContextAccessor = mocks?.HttpContextAccessor ?? new Mock<IHttpContextAccessor>();
            var unitOfWork = mocks?.UnitOfWork ?? new Mock<IUnitOfWork>();

            return new GetUserProfileUseCase(httpContextAccessor.Object, unitOfWork.Object);
        }
    }
}
