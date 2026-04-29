using AuctionHub.Domain.Interfaces.Services.Authentication.Login;
using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Authentication.UserLogin
{
    public record UserLoginUseCaseMocks(
        Mock<IUnitOfWork> UnitOfWork,
        Mock<IPasswordHashService> PasswordHashService,
        Mock<IGenerateTokenService> GenerateTokenService
    );
}
