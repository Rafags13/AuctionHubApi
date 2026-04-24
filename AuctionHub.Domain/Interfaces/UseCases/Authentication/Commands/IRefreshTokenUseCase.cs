using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands
{
    public interface IRefreshTokenUseCase
    {
        Task<OneOf<string, BaseError>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
