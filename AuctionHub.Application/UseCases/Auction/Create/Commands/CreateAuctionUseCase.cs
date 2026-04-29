using AuctionHub.Domain.Constants.Auction;
using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Auction.Create;
using AuctionHub.Domain.Errors.Common.Authentication;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Errors.Common.User;
using AuctionHub.Domain.Helpers.Autentication;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Create.Commands;
using AuctionHub.Infrastructure.Observability;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using Microsoft.AspNetCore.Http;
using OneOf;
using System.Diagnostics;

namespace AuctionHub.Application.UseCases.Auction.Create.Commands
{
    internal sealed class CreateAuctionUseCase(
        IUnitOfWork unitOfWork,
        IBaseEventProducer<CreateAuctionEvent> auctionProducer,
        IHttpContextAccessor httpContextAccessor
    ) : ICreateAuctionUseCase
    {
        public async Task<OneOf<bool, BaseError>> CreateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default)
        {
            var currentUserId = SessionHelper.GetUserId(httpContextAccessor.HttpContext);
            if (!currentUserId.HasValue)
                return new UserIsNotAuthorizedError();

            content.SetSellerId(currentUserId.Value);

            var error = await ValidateAsync(content, cancellationToken);
            if (error != null)
                return error;

            await auctionProducer.DispatchAsync(new CreateAuctionEvent(content), cancellationToken);

            using var activity = Telemetry.ActivitySource.StartActivity(
                "PublishCreateAuction",
                ActivityKind.Producer
            );

            activity?.SetTag("event.type", "CreateAuction");

            return true;
        }

        private async Task<BaseError?> ValidateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(content.Title))
                return new TitleIsRequiredError();

            if (content.Title.Length > AuctionConstants.TITLE_SIZE)
                return new MaxTitleSizeError(AuctionConstants.TITLE_SIZE);

            if (string.IsNullOrEmpty(content.Description))
                return new DescriptionIsRequiredError();

            if (content.Description.Length > AuctionConstants.DESCRIPTION_SIZE)
                return new MaxDescriptionSizeError(AuctionConstants.DESCRIPTION_SIZE);

            if(content.StartingPrice < AuctionConstants.MIN_STARTING_PRICE)
                return new StartingPriceMustBeGreaterThanZeroError();

            if(content.StartTime <= DateTime.UtcNow)
                return new StartTimeMustBeInTheFutureError();

            var userRole = await unitOfWork.UserRepository.GetRoleAsync(content.SellerId, cancellationToken);
            if (!userRole.HasValue)
                return new UserNotFoundError();

            if (userRole.Value != ERole.SELLER)
                return new UserIsntASellerError();

            return null;
        }
    }
}
