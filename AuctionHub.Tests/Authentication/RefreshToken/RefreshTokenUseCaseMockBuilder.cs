using AuctionHub.Domain.DTOs.Authentication.RefreshToken.Response;
using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Interfaces.Services.Authentication.Login;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Tests.Authentication.UserLogin;
using Moq;

namespace AuctionHub.Tests.Authentication.RefreshToken
{
    public class RefreshTokenUseCaseMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IGenerateTokenService> _generateTokenService = new();

        private ResponseRefreshTokenDTO? _refresh;
        private bool _refreshTokenSuccess = true; 

        public RefreshTokenUseCaseMockBuilder()
        {
            _generateTokenService
                .Setup(x => x.GenerateToken(It.IsAny<RequestGenerateTokenDTO>()))
                .Returns("jwt-token");

            _generateTokenService
                .Setup(x => x.GenerateRefreshToken())
                .Returns(new RefreshTokenDTO("refresh-token", DateTime.UtcNow.AddDays(7)));
        }

        public RefreshTokenUseCaseMockBuilder WithValidData()
        {
            _refresh = new ResponseRefreshTokenDTO(1, DateTime.MaxValue);
            _refreshTokenSuccess = true;
            return this;
        }

        public RefreshTokenUseCaseMockBuilder WithInvalidRefreshToken()
        {
            _refresh = null;
            return this;
        }

        public RefreshTokenUseCaseMockBuilder WithExpiredRefreshToken()
        {
            _refresh = new ResponseRefreshTokenDTO(1, DateTime.MinValue);
            return this;
        }

        public RefreshTokenUseCaseMockBuilder WithDatabaseError()
        {
            _refreshTokenSuccess = false;
            return this;
        }

        public RefreshTokenUseCaseMocks Build()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.GetRefreshInformationsAsync(It.IsAny<string>(), default))
                .ReturnsAsync(_refresh);

            _unitOfWork
                .Setup(x => x.UserRepository.RefreshTokenAsync(
                    It.IsAny<RefreshTokenDTO>(),
                    It.IsAny<long>(),
                    default))
                .ReturnsAsync(_refreshTokenSuccess);

            return new RefreshTokenUseCaseMocks(
                _unitOfWork,
                _generateTokenService
            );
        }
    }
}
