using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Interfaces.Services.Authentication.Login;
using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Authentication.UserLogin
{
    public class UserLoginUseCaseMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IPasswordHashService> _passwordHashService = new();
        private readonly Mock<IGenerateTokenService> _generateTokenService = new();

        private RequestGenerateTokenDTO? _user;
        private bool _refreshTokenSuccess = true;

        public UserLoginUseCaseMockBuilder()
        {
            _passwordHashService
                .Setup(x => x.GenerateHash(It.IsAny<string>()))
                .Returns("hashed");

            _generateTokenService
                .Setup(x => x.GenerateToken(It.IsAny<RequestGenerateTokenDTO>()))
                .Returns("jwt-token");

            _generateTokenService
                .Setup(x => x.GenerateRefreshToken())
                .Returns(new RefreshTokenDTO("refresh-token", DateTime.UtcNow.AddDays(7)));
        }

        public UserLoginUseCaseMockBuilder WithValidUser()
        {
            _user = new RequestGenerateTokenDTO(
                Id: 1,
                Name: "Test User",
                Role: ERole.BIDDER,
                Status: EUserStatus.ACTIVE
            );

            return this;
        }

        public UserLoginUseCaseMockBuilder WithBannedUser()
        {
            _user = new RequestGenerateTokenDTO(
                Id: 1,
                Name: "Banned User",
                Role: ERole.BIDDER,
                Status: EUserStatus.BANNED
            );

            return this;
        }

        public UserLoginUseCaseMockBuilder WithUserNotFound()
        {
            _user = null;
            return this;
        }

        public UserLoginUseCaseMockBuilder WithRefreshTokenSuccess()
        {
            _refreshTokenSuccess = true;
            return this;
        }

        public UserLoginUseCaseMockBuilder WithRefreshTokenFailure()
        {
            _refreshTokenSuccess = false;
            return this;
        }

        public UserLoginUseCaseMocks Build()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.GetUserByCredentialsAsync(It.IsAny<RequestUserLoginDTO>(), default))
                .ReturnsAsync(_user);

            _unitOfWork
                .Setup(x => x.UserRepository.RefreshTokenAsync(
                    It.IsAny<RefreshTokenDTO>(),
                    It.IsAny<long>(),
                    default))
                .ReturnsAsync(_refreshTokenSuccess);

            return new UserLoginUseCaseMocks(
                _unitOfWork,
                _passwordHashService,
                _generateTokenService
            );
        }
    }
}
