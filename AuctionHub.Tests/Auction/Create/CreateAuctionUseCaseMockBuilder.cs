using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace AuctionHub.Tests.Auction.Create
{
    public class CreateAuctionUseCaseMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IBaseEventProducer<CreateAuctionEvent>> _auctionProducer = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

        private HttpContext? _context;
        private ERole? _userRole;

        public CreateAuctionUseCaseMockBuilder WithAuthorizedUser(long userId = 1)
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

        public CreateAuctionUseCaseMockBuilder WithUnauthorizedUser()
        {
            _context = null;
            return this;
        }

        public CreateAuctionUseCaseMockBuilder WithSellerRole()
        {
            _userRole = ERole.SELLER;
            return this;
        }

        public CreateAuctionUseCaseMockBuilder WithInvalidRole(ERole role)
        {
            _userRole = role;
            return this;
        }

        public CreateAuctionUseCaseMockBuilder WithUserNotFound()
        {
            _userRole = null;
            return this;
        }

        public CreateAuctionUseCaseMocks Build()
        {
            _httpContextAccessor
                .Setup(x => x.HttpContext)
                .Returns(_context);

            _unitOfWork
                .Setup(x => x.UserRepository.GetRoleAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_userRole);

            _auctionProducer
                .Setup(x => x.DispatchAsync(
                    It.IsAny<CreateAuctionEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            return new CreateAuctionUseCaseMocks(
                _unitOfWork,
                _auctionProducer,
                _httpContextAccessor
            );
        }
    }
}
