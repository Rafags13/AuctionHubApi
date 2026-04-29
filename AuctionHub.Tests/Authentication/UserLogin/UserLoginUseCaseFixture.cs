using AuctionHub.Application.UseCases.Authentication.Commands;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands;

namespace AuctionHub.Tests.Authentication.UserLogin
{
    public class UserLoginUseCaseFixture
    {
        public IUserLoginUseCase Create(UserLoginUseCaseMocks mocks)
        {
            return new UserLoginUseCase(
                mocks.UnitOfWork.Object,
                mocks.PasswordHashService.Object,
                mocks.GenerateTokenService.Object
            );
        }

        public RequestUserLoginDTO CreateValidRequest()
            => new("email@test.com", "123");
    }
}
