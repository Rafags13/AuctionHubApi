using AuctionHub.Domain.DTOs.User.Request.Toggle;
using AuctionHub.Domain.Errors.Common;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.User.Commands
{
    public interface IToggleStatusUserUseCase
    {
        Task<OneOf<bool, BaseError>> ToggleAsync(ToggleUserStatusDTO content, CancellationToken cancellationToken = default);
    }
}
