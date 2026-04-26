using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.Enums.Auction;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Auction.Bid;
using AuctionHub.Domain.Errors.Common.Auction;
using AuctionHub.Domain.Errors.Common.Authentication;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Errors.Common.User;
using AuctionHub.Domain.Helpers.Autentication;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Bid.Commands;
using AuctionHub.Infrastructure.Extensions;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Producer;
using Microsoft.AspNetCore.Http;
using OneOf;

namespace AuctionHub.Application.UseCases.Auction.Bid.Create.Commands
{
    internal sealed class CreateBidUseCase(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IBaseEventProducer<BidAuctionEvent> bidProducer
    ) : ICreateBidUseCase
    {
        public async Task<OneOf<bool, BaseError>> BidAsync(BidRequestDTO content, CancellationToken cancellationToken = default)
        {
            var currentUserId = SessionHelper.GetUserId(httpContextAccessor.HttpContext);
            var response = await ValidateAsync(content, currentUserId, cancellationToken);
            if (response.IsError())
                return response.GetError();

            var outBidId = response.GetValue();

            content.SetBidderId(currentUserId!.Value);
            content.SetOutBidId(outBidId);

            await bidProducer.DispatchAsync(new BidAuctionEvent(content), cancellationToken);

            return true;
        }

        private async Task<OneOf<long?, BaseError>> ValidateAsync(BidRequestDTO content, long? userId, CancellationToken cancellationToken)
        {
            var userError = await ValidateCurrentUserAsync(userId, cancellationToken);
            if (userError != null)
                return userError;

            var currentAuctionInfo = await unitOfWork.AuctionRepository.GetAuctionBidInformationsAsync(content.AuctionId, cancellationToken);

            if (currentAuctionInfo == null)
                return new AuctionNotFoundError();

            if (currentAuctionInfo.Status != EAuctionStatus.OPEN)
                return new AuctionNotOpenedError();

            var lastBidInfo = await unitOfWork.BidRepository.GetBidToOutBidAsync(content.AuctionId, cancellationToken);

            if (lastBidInfo == null && currentAuctionInfo.StartingPrice > content.Amount)
                return new BidBelowStartingPriceError();

            if (lastBidInfo?.Amount >= content.Amount)
                return new AmountShouldBeHigherThenLastBidError();

            return lastBidInfo?.Id;
        }

        private async Task<BaseError?> ValidateCurrentUserAsync(long? userId, CancellationToken cancellationToken)
        {
            if (!userId.HasValue)
                return new UserIsNotAuthorizedError();

            var currentRole = await unitOfWork.UserRepository.GetRoleAsync(userId.Value, cancellationToken);

            if (!currentRole.HasValue)
                return new UserNotFoundError();

            if (currentRole != ERole.BIDDER)
                return new CurrentUserIsntABidderError();

            return null;
        }
    }
}
