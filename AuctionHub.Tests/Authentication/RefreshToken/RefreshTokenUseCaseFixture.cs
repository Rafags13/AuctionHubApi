using AuctionHub.Application.UseCases.Authentication.Commands;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands;

namespace AuctionHub.Tests.Authentication.RefreshToken
{
    public class RefreshTokenUseCaseFixture
    {
        public IRefreshTokenUseCase Create(RefreshTokenUseCaseMocks mocks)
        {
            return new RefreshTokenUseCase(
                mocks.UnitOfWork.Object,
                mocks.GenerateTokenService.Object
            );
        }

        public string CreateValidRequest()
            => "valid-refresh-token";
    }
}
