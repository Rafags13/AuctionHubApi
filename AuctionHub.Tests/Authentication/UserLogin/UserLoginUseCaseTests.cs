using AuctionHub.Domain.Errors.Authentication.Login;
using AuctionHub.Domain.Errors.Common.Base;
using FluentAssertions;

namespace AuctionHub.Tests.Authentication.UserLogin
{
    public class UserLoginUseCaseTests
    {
        private readonly UserLoginUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Success_When_Credentials_Are_Valid()
        {
            var mocks = new UserLoginUseCaseMockBuilder()
                .WithValidUser()
                .WithRefreshTokenSuccess()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.LoginAsync(request);

            result.IsT0.Should().BeTrue();
            result.AsT0.Token.Should().Be("jwt-token");
            result.AsT0.RefreshToken.Should().Be("refresh-token");
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Not_Found()
        {
            var mocks = new UserLoginUseCaseMockBuilder()
                .WithUserNotFound()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.LoginAsync(request);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<UserOrPasswordIsIncorrectError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Is_Banned()
        {
            var mocks = new UserLoginUseCaseMockBuilder()
                .WithBannedUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.LoginAsync(request);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<UserIsBannedError>();
        }

        [Fact]
        public async Task Should_Return_DatabaseError_When_RefreshToken_Fails()
        {
            var mocks = new UserLoginUseCaseMockBuilder()
                .WithValidUser()
                .WithRefreshTokenFailure()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateValidRequest();

            var result = await sut.LoginAsync(request);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<DatabaseError>();
        }
    }
}
