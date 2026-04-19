using AuctionHub.Domain.DTOs.User.Request.Create;
using AuctionHub.Domain.Errors.Common;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.User.Commands
{
    public interface IRegisterBidderUseCase
    {
        Task<OneOf<bool, BaseError>> RegisterAsync(RequestCreateBidderDTO content, CancellationToken cancellationToken = default);
    }
}
