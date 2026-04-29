using AuctionHub.Domain.Constants.Auction;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Auction.Create;
using AuctionHub.Domain.Errors.Common.Authentication;
using AuctionHub.Domain.Errors.Common.User;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using FluentAssertions;
using Moq;

namespace AuctionHub.Tests.Auction.Create
{
    public class CreateAuctionUseCaseTests
    {
        private readonly CreateAuctionUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Success_When_Auction_Is_Valid()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithSellerRole()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.CreateAsync(request);

            result.IsT0.Should().BeTrue();

            mocks.AuctionProducer.Verify(x =>
                x.DispatchAsync(It.IsAny<CreateAuctionEvent>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Is_Not_Authorized()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithUnauthorizedUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.CreateAsync(request);

            result.AsT1.Should().BeOfType<UserIsNotAuthorizedError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Title_Is_Empty()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithSellerRole()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var requestEmptyTitle = request with { Title = "" };

            var result = await sut.CreateAsync(requestEmptyTitle);

            result.AsT1.Should().BeOfType<TitleIsRequiredError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Title_Is_Too_Long()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithSellerRole()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var requestTitle = request with
            {
                Title = new string('a', AuctionConstants.TITLE_SIZE + 1)
            };

            var result = await sut.CreateAsync(requestTitle);

            result.AsT1.Should().BeOfType<MaxTitleSizeError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Description_Is_Empty()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithSellerRole()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var requestEmptyDescription = request with { Description = "" };

            var result = await sut.CreateAsync(requestEmptyDescription);

            result.AsT1.Should().BeOfType<DescriptionIsRequiredError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Description_Is_Too_Long()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithSellerRole()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var requestDescription = request with
            {
                Description = new string('a', AuctionConstants.DESCRIPTION_SIZE + 1)
            };

            var result = await sut.CreateAsync(requestDescription);

            result.AsT1.Should().BeOfType<MaxDescriptionSizeError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_StartingPrice_Is_Invalid()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithSellerRole()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var requestZeroPrice = request with { StartingPrice = 0 };

            var result = await sut.CreateAsync(requestZeroPrice);

            result.AsT1.Should().BeOfType<StartingPriceMustBeGreaterThanZeroError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_StartTime_Is_Invalid()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithSellerRole()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var requestLateStartTime = request with
            {
                StartTime = DateTime.UtcNow.AddHours(-1)
            };

            var result = await sut.CreateAsync(requestLateStartTime);

            result.AsT1.Should().BeOfType<StartTimeMustBeInTheFutureError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Not_Found()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithUserNotFound()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.CreateAsync(request);

            result.AsT1.Should().BeOfType<UserNotFoundError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Is_Not_Seller()
        {
            var mocks = new CreateAuctionUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithInvalidRole(ERole.BIDDER)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.CreateAsync(request);

            result.AsT1.Should().BeOfType<UserIsntASellerError>();
        }
    }
}
