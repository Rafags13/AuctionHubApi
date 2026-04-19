using AuctionHub.Domain.DTOs.User.Request.Create;
using AuctionHub.Domain.Errors.Common;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.User.Commands
{
    public interface IRegisterSellerUseCase
    {
        Task<OneOf<bool, BaseError>> RegisterAsync(RequestCreateSellerDTO content, CancellationToken cancellationToken = default);
    }
}
