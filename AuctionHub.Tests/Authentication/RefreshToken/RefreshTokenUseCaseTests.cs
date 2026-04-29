using AuctionHub.Domain.Errors.Authentication.RefreshToken;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Tests.Authentication.Register.Bidder;
using FluentAssertions;

namespace AuctionHub.Tests.Authentication.RefreshToken
{
    public class RefreshTokenUseCaseTests
    {
        private readonly RefreshTokenUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Register_Successfully_When_Data_Is_Valid()
        {
            var mocks = new RefreshTokenUseCaseMockBuilder()
                .WithValidData()
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await sut.RefreshTokenAsync(request, default);

            result.IsT0.Should().BeTrue();
            result.AsT0.Should().BeEquivalentTo("refresh-token");
        }

        [Fact]
        public async Task Should_Not_Register_When_Refresh_Token_Is_Invalid()
        {
            var mocks = new RefreshTokenUseCaseMockBuilder()
                .WithInvalidRefreshToken()
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await sut.RefreshTokenAsync(request, default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<InvalidRefreshTokenError>();
        }

        [Fact]
        public async Task Should_Not_Register_When_Refresh_Token_Is_Expired()
        {
            var mocks = new RefreshTokenUseCaseMockBuilder()
                .WithExpiredRefreshToken()
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await sut.RefreshTokenAsync(request, default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<ExpiredRefreshTokenError>();
        }

        [Fact]
        public async Task Should_Not_Register_When_Database_Fails()
        {
            var mocks = new RefreshTokenUseCaseMockBuilder()
                .WithValidData()
                .WithDatabaseError()
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await sut.RefreshTokenAsync(request, default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<DatabaseError>();
        }
    }
}
