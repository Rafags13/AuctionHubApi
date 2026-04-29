using AuctionHub.Domain.DTOs.Auction.Details.Response;
using AuctionHub.Domain.Errors.Common.Auction;
using FluentAssertions;

namespace AuctionHub.Tests.Auction.Details
{
    public class GetAuctionInformationsUseCaseTests
    {
        private readonly GetAuctionInformationsUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Auction_When_Auction_Exists()
        {
            var mocks = new GetAuctionInformationsUseCaseMockBuilder()
                .WithAuction()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.IsT0.Should().BeTrue();
            result.AsT0.Should().BeOfType<AuctionDetailsResponseDTO>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Auction_Not_Found()
        {
            var mocks = new GetAuctionInformationsUseCaseMockBuilder()
                .WithoutAuction()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<AuctionNotFoundError>();
        }
    }
}
