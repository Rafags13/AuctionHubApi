using AuctionHub.Domain.DTOs.User.Profile.Response;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AuctionHub.Tests.User.Profile
{
    public class GetUserProfileUseCaseMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

        private UserProfileDTO? _user;
        private HttpContext? _context;

        public GetUserProfileUseCaseMockBuilder GetValidContext()
        {
            _context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            };
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "1")
            };
            _context.User.AddIdentity(new ClaimsIdentity(claims, "Bearer"));

            return this;
        }

        public GetUserProfileUseCaseMockBuilder GetInvalidContext()
        {
            _context = null;

            return this;
        }

        public GetUserProfileUseCaseMockBuilder GetValidUser()
        {
            _user = new UserProfileDTO(
                "teste",
                "teste@gmail.com",
                ERole.BIDDER,
                EUserStatus.ACTIVE
            );
            return this;
        }

        public GetUserProfileUseCaseMockBuilder GetInvalidUser()
        {
            _user = null;
            return this;
        }

        public GetUserProfileUseCaseMocks Build()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.GetUserProfileAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_user);

            _httpContextAccessor.Setup(x => x.HttpContext)
                .Returns(_context);

            return new GetUserProfileUseCaseMocks(_httpContextAccessor, _unitOfWork);
        }
    }
}
