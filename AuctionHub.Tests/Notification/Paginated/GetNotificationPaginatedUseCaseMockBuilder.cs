using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.DTOs.Notification.Paginated.Response;
using AuctionHub.Domain.Enums.Notification;
using AuctionHub.Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace AuctionHub.Tests.Notification.Paginated
{
    public class GetNotificationPaginatedUseCaseMockBuilder
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private HttpContext? _context;
        private PaginatedDTO<PaginatedNotificationResponseDTO> _response =
            new([], 0, 0, 0);

        public GetNotificationPaginatedUseCaseMockBuilder WithAuthorizedUser(long userId = 1)
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

        public GetNotificationPaginatedUseCaseMockBuilder WithUnauthorizedUser()
        {
            _context = null;
            return this;
        }

        public GetNotificationPaginatedUseCaseMockBuilder WithPaginatedResponse(
            PaginatedDTO<PaginatedNotificationResponseDTO>? response = null)
        {
            _response = response ?? new PaginatedDTO<PaginatedNotificationResponseDTO>(
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

            return this;
        }

        public GetNotificationPaginatedUseCaseMocks Build()
        {
            _httpContextAccessor
                .Setup(x => x.HttpContext)
                .Returns(_context);

            _unitOfWork
                .Setup(x => x.NotificationRepository.GetPaginatedAsync(
                    It.IsAny<PaginatedNotificationRequestDTO>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_response);

            return new GetNotificationPaginatedUseCaseMocks(
                _httpContextAccessor,
                _unitOfWork
            );
        }
    }
}
