using AuctionHub.Domain.DTOs.User.Toggle.Request;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.User.Commands
{
    public interface IToggleStatusUserUseCase
    {
        Task<OneOf<bool, BaseError>> ToggleAsync(RequestToggleUserStatusDTO content, CancellationToken cancellationToken = default);
    }
}
