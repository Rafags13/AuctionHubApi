using AuctionHub.Domain.DTOs.User.Profile.Response;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.UseCases.User.Queries
{
    public interface IGetUserProfileUseCase
    {
        Task<OneOf<UserProfileDTO, BaseError>> GetAsync(CancellationToken cancellationToken = default);
    }
}
