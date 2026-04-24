using AuctionHub.Domain.Errors.Common;
using AuctionHub.Domain.Errors.User;
using AuctionHub.Domain.Interfaces.Services.User.Login;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using OneOf;

namespace AuctionHub.Application.UseCases.User.Commands
{
    internal sealed class RefreshTokenUseCase(
        IUnitOfWork unitOfWork,
        IGenerateTokenService generateTokenService
    ) : IRefreshTokenUseCase
    {
        public async Task<OneOf<string, BaseError>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var refreshTokenInformations = await unitOfWork.UserRepository.GetRefreshInformationsAsync(refreshToken, cancellationToken);

            if (refreshTokenInformations is null)
                return new InvalidRefreshTokenError();

            if (refreshTokenInformations.ExpirationDateTime <= DateTime.UtcNow)
                return new ExpiredRefreshTokenError();

            var newRefreshToken = generateTokenService.GenerateRefreshToken();

            if (!await unitOfWork.UserRepository.RefreshTokenAsync(newRefreshToken, refreshTokenInformations.UserId, cancellationToken))
                return new DatabaseError();

            return newRefreshToken.RefreshToken;
        }
    }
}
