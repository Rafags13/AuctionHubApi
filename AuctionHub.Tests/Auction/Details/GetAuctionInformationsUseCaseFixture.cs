using AuctionHub.Application.UseCases.Auction.Details.Queries;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Details.Queries;
using Moq;

namespace AuctionHub.Tests.Auction.Details
{
    public class GetAuctionInformationsUseCaseFixture
    {
        public IGetAuctionInformationsUseCase Create(GetAuctionInformationsUseCaseMocks? mocks = null)
        {
            var unitOfWork = mocks?.UnitOfWork ?? new Mock<IUnitOfWork>();

            return new GetAuctionInformationsUseCase(unitOfWork.Object);
        }
    }
}
