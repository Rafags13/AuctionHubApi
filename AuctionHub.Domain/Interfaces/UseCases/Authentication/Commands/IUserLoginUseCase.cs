using AuctionHub.Domain.DTOs.Authentication.Login.Response;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands
{
    public interface IUserLoginUseCase
    {
        Task<OneOf<ResponseUserLoginDTO, BaseError>> LoginAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default);
    }
}
