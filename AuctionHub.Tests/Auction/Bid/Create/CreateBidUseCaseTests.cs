using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Auction.Bid;
using AuctionHub.Domain.Errors.Common.Auction;
using AuctionHub.Domain.Errors.Common.Authentication;
using AuctionHub.Domain.Errors.Common.User;
using FluentAssertions;

namespace AuctionHub.Tests.Auction.Bid.Create
{
    public class CreateBidUseCaseTests
    {
        private readonly CreateBidUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Success_When_Bid_Is_Valid()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithOpenAuction()
                .WithLastBid(100)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.BidAsync(request, default);

            result.IsT0.Should().BeTrue();
            result.AsT0.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Is_Not_Authorized()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithUnauthorizedUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.BidAsync(request, default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<UserIsNotAuthorizedError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Not_Found()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithUserNotFound()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.BidAsync(request, default);

            result.AsT1.Should().BeOfType<UserNotFoundError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Is_Not_Bidder()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithAuthorizedUser(role: ERole.ADMIN)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.BidAsync(request, default);

            result.AsT1.Should().BeOfType<CurrentUserIsntABidderError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Auction_Not_Found()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithAuctionNotFound()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.BidAsync(request, default);

            result.AsT1.Should().BeOfType<AuctionNotFoundError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Auction_Not_Open()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithClosedAuction()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.BidAsync(request, default);

            result.AsT1.Should().BeOfType<AuctionNotOpenedError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Bid_Is_Below_Starting_Price()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithOpenAuction(startingPrice: 200)
                .WithLastBid(null)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest(amount: 100);

            var result = await sut.BidAsync(request, default);

            result.AsT1.Should().BeOfType<BidBelowStartingPriceError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Bid_Is_Lower_Than_Last_Bid()
        {
            var mocks = new CreateBidUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithOpenAuction()
                .WithLastBid(200)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest(amount: 150);

            var result = await sut.BidAsync(request, default);

            result.AsT1.Should().BeOfType<AmountShouldBeHigherThenLastBidError>();
        }
    }
}
