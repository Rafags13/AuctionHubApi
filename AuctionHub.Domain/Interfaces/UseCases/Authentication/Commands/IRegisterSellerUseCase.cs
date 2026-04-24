using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands
{
    public interface IRegisterSellerUseCase
    {
        Task<OneOf<bool, BaseError>> RegisterAsync(RequestCreateSellerDTO content, CancellationToken cancellationToken = default);
    }
}
