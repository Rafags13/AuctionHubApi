using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Authentication.ValidateRegister
{
    public record ValidateRegisterMocks(
        Mock<IUnitOfWork> UnitOfWork
    );
}
