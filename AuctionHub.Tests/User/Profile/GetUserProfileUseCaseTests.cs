using AuctionHub.Domain.DTOs.User.Profile.Response;
using AuctionHub.Domain.Errors.Common.Authentication;
using AuctionHub.Domain.Errors.Common.User;
using FluentAssertions;

namespace AuctionHub.Tests.User.Profile
{
    public class GetUserProfileUseCaseTests
    {
        private readonly GetUserProfileUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Success_When_User_Exists_And_Successful_Database_Operation()
        {
            var mocks = new GetUserProfileUseCaseMockBuilder()
                .GetValidContext()
                .GetValidUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(default);

            result.IsT0.Should().BeTrue();
            result.AsT0.Should().BeOfType<UserProfileDTO>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Isnt_Authorized()
        {
            var mocks = new GetUserProfileUseCaseMockBuilder()
                .GetInvalidContext()
                .GetValidUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<UserIsNotAuthorizedError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Doesnt_Exists()
        {
            var mocks = new GetUserProfileUseCaseMockBuilder()
                .GetValidContext()
                .GetInvalidUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<UserNotFoundError>();
        }
    }
}
