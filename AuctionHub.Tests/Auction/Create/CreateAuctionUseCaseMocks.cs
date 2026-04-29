using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Auction.Create
{
    public record CreateAuctionUseCaseMocks(
        Mock<IUnitOfWork> UnitOfWork,
        Mock<IBaseEventProducer<CreateAuctionEvent>> AuctionProducer,
        Mock<IHttpContextAccessor> HttpContextAccessor
    );
}
