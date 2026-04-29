using AuctionHub.Domain.DTOs.Notification.Read.Request;
using AuctionHub.Domain.Enums.Notification;
using AuctionHub.Domain.Errors.Common.Notification;
using AuctionHub.Domain.Errors.Notification.Read;
using FluentAssertions;
using Moq;

namespace AuctionHub.Tests.Notification.Read
{
    public class GetNotificationDetailsUseCaseTests
    {
        private readonly GetNotificationDetailsUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Error_When_User_Is_Not_Authorized()
        {
            var mocks = new GetNotificationDetailsUseCaseMockBuilder()
                .WithUnauthorizedUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<NotificationDoesntBelongToCurrentUserError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Has_No_Access_To_Notification()
        {
            var mocks = new GetNotificationDetailsUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithAccessDenied()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.AsT1.Should().BeOfType<NotificationDoesntBelongToCurrentUserError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Notification_Not_Found()
        {
            var mocks = new GetNotificationDetailsUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithoutNotification()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.AsT1.Should().BeOfType<NotificationNotFoundError>();
        }

        [Fact]
        public async Task Should_Mark_As_Read_When_Notification_Is_Unread()
        {
            var mocks = new GetNotificationDetailsUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithNotificationUnread()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.IsT0.Should().BeTrue();

            mocks.UnitOfWork.Verify(x =>
                x.NotificationRepository.ReadAsync(
                    It.IsAny<ReadNotificationRequestDTO>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_Not_Mark_As_Read_When_Notification_Is_Already_Read()
        {
            var mocks = new GetNotificationDetailsUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithNotificationRead()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.IsT0.Should().BeTrue();

            mocks.UnitOfWork.Verify(x =>
                x.NotificationRepository.ReadAsync(
                    It.IsAny<ReadNotificationRequestDTO>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_Return_Notification_When_Valid()
        {
            var mocks = new GetNotificationDetailsUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithNotificationRead()
                .Build();

            var sut = _fixture.Create(mocks);

            var result = await sut.GetAsync(1);

            result.IsT0.Should().BeTrue();

            var notification = result.AsT0;
            notification.Message.Should().Be("Test message");
            notification.Type.Should().Be(ENotificationType.WIN);
        }
    }
}
