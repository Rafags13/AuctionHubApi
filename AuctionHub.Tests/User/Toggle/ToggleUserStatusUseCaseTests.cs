using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Errors.Common.User;
using FluentAssertions;

namespace AuctionHub.Tests.User.Toggle
{
    public class ToggleUserStatusUseCaseTests
    {
        private readonly ToggleUserStatusUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Success_When_User_Exists_And_Successful_Database_Operation()
        {
            var mocks = new ToggleUserStatusUseCaseMockBuilder()
                .UserExists(true)
                .SuccessfulToggle(true)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.GetValidRequest();

            var result = await sut.ToggleAsync(request);

            result.IsT0.Should().BeTrue();
            result.AsT0.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Doesnt_Exists()
        {
            var mocks = new ToggleUserStatusUseCaseMockBuilder()
                .UserExists(false)
                .SuccessfulToggle(true)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.GetValidRequest();

            var result = await sut.ToggleAsync(request);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<UserNotFoundError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Database_Operation_Fails()
        {
            var mocks = new ToggleUserStatusUseCaseMockBuilder()
                .UserExists(true)
                .SuccessfulToggle(false)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.GetValidRequest();

            var result = await sut.ToggleAsync(request);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<DatabaseError>();
        }
    }
}
