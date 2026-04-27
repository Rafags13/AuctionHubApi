using AuctionHub.Domain.DTOs.User.Profile.Response;
using AuctionHub.Domain.Errors.Common.Authentication;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Errors.Common.User;
using AuctionHub.Domain.Helpers.Autentication;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.User.Queries;
using Microsoft.AspNetCore.Http;
using OneOf;

namespace AuctionHub.Application.UseCases.User.Queries
{
    internal sealed class GetUserProfileUseCase(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork
    ) : IGetUserProfileUseCase
    {
        public async Task<OneOf<UserProfileDTO, BaseError>> GetAsync(CancellationToken cancellationToken = default)
        {
            var userId = SessionHelper.GetUserId(httpContextAccessor.HttpContext);
            if (!userId.HasValue)
                return new UserIsNotAuthorizedError();

            var user = await unitOfWork.UserRepository.GetUserProfileAsync(userId.Value, cancellationToken);
            if (user == null)
                return new UserNotFoundError();

            return user;
        }
    }
}
