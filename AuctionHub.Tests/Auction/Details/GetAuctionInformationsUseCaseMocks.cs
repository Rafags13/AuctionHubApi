using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Auction.Details
{
    public record GetAuctionInformationsUseCaseMocks(
        Mock<IUnitOfWork> UnitOfWork
    );
}
