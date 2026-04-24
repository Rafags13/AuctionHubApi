using AuctionHub.Domain.Errors.Common;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.User.Commands
{
    public interface IRefreshTokenUseCase
    {
        Task<OneOf<string, BaseError>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
