using AuctionHub.Domain.DTOs.Notification.Read.Request;
using AuctionHub.Domain.DTOs.Notification.Read.Response;
using AuctionHub.Domain.Enums.Notification;
using AuctionHub.Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace AuctionHub.Tests.Notification.Read
{
    public class GetNotificationDetailsUseCaseMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

        private HttpContext? _context;
        private bool _isAllowed = true;
        private ReadNotificationResponseDTO? _notification;

        public GetNotificationDetailsUseCaseMockBuilder WithAuthorizedUser(long userId = 1)
        {
            _context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            };

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

            _context.User.AddIdentity(new ClaimsIdentity(claims, "Bearer"));

            return this;
        }

        public GetNotificationDetailsUseCaseMockBuilder WithUnauthorizedUser()
        {
            _context = null;
            return this;
        }

        public GetNotificationDetailsUseCaseMockBuilder WithAccessDenied()
        {
            _isAllowed = false;
            return this;
        }

        public GetNotificationDetailsUseCaseMockBuilder WithNotificationUnread()
        {
            _notification = new ReadNotificationResponseDTO(
                Type: ENotificationType.WIN,
                Message: "Test message",
                ReadAt: null,
                CreatedAt: DateTime.UtcNow.AddMinutes(-10)
            );

            return this;
        }

        public GetNotificationDetailsUseCaseMockBuilder WithNotificationRead()
        {
            _notification = new ReadNotificationResponseDTO(
                Type: ENotificationType.WIN,
                Message: "Test message",
                ReadAt: DateTime.UtcNow.AddMinutes(-5),
                CreatedAt: DateTime.UtcNow.AddMinutes(-10)
            );

            return this;
        }

        public GetNotificationDetailsUseCaseMockBuilder WithoutNotification()
        {
            _notification = null;
            return this;
        }

        public GetNotificationDetailsUseCaseMocks Build()
        {
            _httpContextAccessor
                .Setup(x => x.HttpContext)
                .Returns(_context);

            _unitOfWork
                .Setup(x => x.NotificationRepository.IsAllowedToReadAsync(
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_isAllowed);

            _unitOfWork
                .Setup(x => x.NotificationRepository.GetAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_notification);

            _unitOfWork
                .Setup(x => x.NotificationRepository.ReadAsync(
                    It.IsAny<ReadNotificationRequestDTO>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(It.IsAny<bool>()));

            return new GetNotificationDetailsUseCaseMocks(
                _unitOfWork,
                _httpContextAccessor
            );
        }
    }
}
