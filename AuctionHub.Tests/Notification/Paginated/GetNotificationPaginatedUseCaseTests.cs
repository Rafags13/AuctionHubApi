using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.DTOs.Notification.Paginated.Response;
using AuctionHub.Domain.Enums.Notification;
using FluentAssertions;
using Moq;

namespace AuctionHub.Tests.Notification.Paginated
{
    public class GetNotificationPaginatedUseCaseTests
    {
        private readonly GetNotificationPaginatedUseCaseFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Empty_Result_When_User_Is_Not_Authorized()
        {
            var mocks = new GetNotificationPaginatedUseCaseMockBuilder()
                .WithUnauthorizedUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateRequest();

            var result = await sut.GetPaginatedAsync(request);

            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
        }

        [Fact]
        public async Task Should_Return_Paginated_Data_When_User_Is_Authorized()
        {
            var expected = new PaginatedDTO<PaginatedNotificationResponseDTO>(
                new List<PaginatedNotificationResponseDTO>
                {
            new(
                Id: 1,
                Type: ENotificationType.WIN,
                ReadAt: null
            )
                },
                TotalItems: 1,
                Page: 1,
                PageSize: 10
            );

            var mocks = new GetNotificationPaginatedUseCaseMockBuilder()
                .WithAuthorizedUser()
                .WithPaginatedResponse(expected)
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateRequest();

            var result = await sut.GetPaginatedAsync(request);

            result.Items.Should().HaveCount(1);
            result.Items.First().Id.Should().Be(1);
            result.TotalItems.Should().Be(1);
        }

        [Fact]
        public async Task Should_Call_Repository_When_User_Is_Authorized()
        {
            var mocks = new GetNotificationPaginatedUseCaseMockBuilder()
                .WithAuthorizedUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateRequest();

            await sut.GetPaginatedAsync(request);

            mocks.UnitOfWork.Verify(x =>
                x.NotificationRepository.GetPaginatedAsync(
                    It.IsAny<PaginatedNotificationRequestDTO>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Should_Not_Call_Repository_When_User_Is_Not_Authorized()
        {
            var mocks = new GetNotificationPaginatedUseCaseMockBuilder()
                .WithUnauthorizedUser()
                .Build();

            var sut = _fixture.Create(mocks);

            var request = _fixture.CreateRequest();

            await sut.GetPaginatedAsync(request);

            mocks.UnitOfWork.Verify(x =>
                x.NotificationRepository.GetPaginatedAsync(
                    It.IsAny<PaginatedNotificationRequestDTO>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
