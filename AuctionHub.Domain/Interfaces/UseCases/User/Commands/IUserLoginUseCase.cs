using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Errors.Common;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.User.Commands
{
    public interface IUserLoginUseCase
    {
        Task<OneOf<string, BaseError>> LoginAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default);
    }
}
