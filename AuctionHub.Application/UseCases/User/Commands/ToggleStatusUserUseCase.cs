using AuctionHub.Domain.DTOs.User.Toggle.Request;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Errors.Common.User;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using OneOf;

namespace AuctionHub.Application.UseCases.User.Commands
{
    internal sealed class ToggleStatusUserUseCase(
        IUnitOfWork unitOfWork
    ) : IToggleStatusUserUseCase
    {
        public async Task<OneOf<bool, BaseError>> ToggleAsync(RequestToggleUserStatusDTO content, CancellationToken cancellationToken = default)
        {
            if (!await unitOfWork.UserRepository.AnyAsync(u => u.Id == content.UserId, cancellationToken))
                return new UserNotFoundError();

            if(await unitOfWork.UserRepository.ToggleAsync(content, cancellationToken) <= 0)
                return new DatabaseError();

            return true;
        }
    }
}
