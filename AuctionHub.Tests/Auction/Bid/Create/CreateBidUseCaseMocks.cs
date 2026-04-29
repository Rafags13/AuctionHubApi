using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Producer;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Auction.Bid.Create
{
    public record CreateBidUseCaseMocks(
        Mock<IHttpContextAccessor> HttpContextAccessor,
        Mock<IUnitOfWork> UnitOfWork,
        Mock<IBaseEventProducer<BidAuctionEvent>> BidProducer
    );
}
