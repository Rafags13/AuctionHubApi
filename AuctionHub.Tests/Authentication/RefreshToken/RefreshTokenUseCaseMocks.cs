using AuctionHub.Domain.Interfaces.Services.Authentication.Login;
using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Authentication.RefreshToken
{
    public record RefreshTokenUseCaseMocks(
        Mock<IUnitOfWork> UnitOfWork,
        Mock<IGenerateTokenService> GenerateTokenService
    );
}
