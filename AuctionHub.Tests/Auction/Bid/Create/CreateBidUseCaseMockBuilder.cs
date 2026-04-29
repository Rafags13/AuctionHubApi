using AuctionHub.Domain.DTOs.Auction.Bid.Response;
using AuctionHub.Domain.Enums.Auction;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Producer;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace AuctionHub.Tests.Auction.Bid.Create
{
    public class CreateBidUseCaseMockBuilder
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IBaseEventProducer<BidAuctionEvent>> _bidProducer = new();

        private HttpContext? _context;
        private ERole? _role;
        private AuctionBidInformationsDTO? _auction;
        private decimal? _lastBid;

        public CreateBidUseCaseMockBuilder WithAuthorizedUser(long userId = 1, ERole role = ERole.BIDDER)
        {
            _role = role;

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

        public CreateBidUseCaseMockBuilder WithUnauthorizedUser()
        {
            _context = null;
            return this;
        }

        public CreateBidUseCaseMockBuilder WithUserNotFound()
        {
            _role = null;
            return this;
        }

        public CreateBidUseCaseMockBuilder WithInvalidRole(ERole role)
        {
            _role = role;
            return this;
        }

        public CreateBidUseCaseMockBuilder WithOpenAuction(decimal startingPrice = 100)
        {
            _auction = new AuctionBidInformationsDTO(EAuctionStatus.OPEN, startingPrice);

            return this;
        }

        public CreateBidUseCaseMockBuilder WithClosedAuction()
        {
            _auction = new AuctionBidInformationsDTO(EAuctionStatus.CLOSED, 100);

            return this;
        }

        public CreateBidUseCaseMockBuilder WithAuctionNotFound()
        {
            _auction = null;
            return this;
        }

        public CreateBidUseCaseMockBuilder WithLastBid(decimal? lastBid)
        {
            _lastBid = lastBid;
            return this;
        }

        public CreateBidUseCaseMocks Build()
        {
            _httpContextAccessor
                .Setup(x => x.HttpContext)
                .Returns(_context);

            _unitOfWork
                .Setup(x => x.UserRepository.GetRoleAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_role);

            _unitOfWork
                .Setup(x => x.AuctionRepository.GetAuctionBidInformationsAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_auction);

            _unitOfWork
                .Setup(x => x.BidRepository.GetLastBidAmountAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_lastBid);

            _bidProducer
                .Setup(x => x.DispatchAsync(
                    It.IsAny<BidAuctionEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            return new CreateBidUseCaseMocks(
                _httpContextAccessor,
                _unitOfWork,
                _bidProducer
            );
        }
    }
}
