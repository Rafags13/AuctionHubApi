using AuctionHub.Application.UseCases.Auction.Bid.Create.Commands;
using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Bid.Commands;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Producer;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Auction.Bid.Create
{
    public class CreateBidUseCaseFixture
    {
        public ICreateBidUseCase Create(CreateBidUseCaseMocks? mocks = null)
        {
            var httpContextAccessor = mocks?.HttpContextAccessor ?? new Mock<IHttpContextAccessor>();
            var unitOfWork = mocks?.UnitOfWork ?? new Mock<IUnitOfWork>();
            var bidProducer = mocks?.BidProducer ?? new Mock<IBaseEventProducer<BidAuctionEvent>>();

            return new CreateBidUseCase(
                httpContextAccessor.Object,
                unitOfWork.Object,
                bidProducer.Object
            );
        }

        public BidRequestDTO CreateValidRequest(decimal amount = 150)
            => new (1, amount);
    }
}
