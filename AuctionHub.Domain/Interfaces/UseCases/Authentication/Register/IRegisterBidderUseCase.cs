using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.Authentication.Register
{
    public interface IRegisterBidderUseCase
    {
        Task<OneOf<bool, BaseError>> RegisterAsync(RequestCreateBidderDTO content, CancellationToken cancellationToken = default);
    }
}
