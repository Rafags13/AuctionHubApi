using AuctionHub.Application.UseCases.Auction.Create.Commands;
using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Create.Commands;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AuctionHub.Tests.Auction.Create
{
    public class CreateAuctionUseCaseFixture
    {
        public ICreateAuctionUseCase Create(CreateAuctionUseCaseMocks? mocks = null)
        {
            var unitOfWork = mocks?.UnitOfWork ?? new Mock<IUnitOfWork>();
            var auctionProducer = mocks?.AuctionProducer ?? new Mock<IBaseEventProducer<CreateAuctionEvent>>();
            var httpContextAccessor = mocks?.HttpContextAccessor ?? new Mock<IHttpContextAccessor>();

            return new CreateAuctionUseCase(
                unitOfWork.Object,
                auctionProducer.Object,
                httpContextAccessor.Object
            );
        }

        public RequestCreateAuctionDTO CreateValidRequest()
        => new(
            "Valid Title",
            "Valid Description",
            100,
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(2),
            1
        );
    }
}
