using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.User.Toggle
{
    public record ToggleUserStatusUseCaseMocks(Mock<IUnitOfWork> UnitOfWork);
}
