using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.DTOs.User.Response;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Common;
using AuctionHub.Domain.Errors.User;
using AuctionHub.Domain.Interfaces.Services.User.Login;
using AuctionHub.Domain.Interfaces.Services.User.Password;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using AuctionHub.Infrastructure.Extensions;
using OneOf;

namespace AuctionHub.Application.UseCases.User.Commands
{
    internal sealed class UserLoginUseCase(
        IUnitOfWork unitOfWork,
        IPasswordHashService passwordHashService,
        IGenerateTokenService generateTokenService) : IUserLoginUseCase
    {
        public async Task<OneOf<ResponseUserLoginDTO, BaseError>> LoginAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default)
        {
            var hashPassword = passwordHashService.GenerateHash(content.Password);
            content.HashPassword(hashPassword);

            var userToLogin = await GetUserByCredentials(content, cancellationToken);
            if (userToLogin.IsError())
                return userToLogin.GetError();

            var userToLoginInformations = userToLogin.GetValue();
            var refreshTokenInformations = generateTokenService.GenerateRefreshToken();

            if (!await unitOfWork.UserRepository.RefreshTokenAsync(refreshTokenInformations, userToLoginInformations.Id, cancellationToken))
                return new DatabaseError();

            return new ResponseUserLoginDTO(generateTokenService.GenerateToken(userToLoginInformations), refreshTokenInformations.RefreshToken);
        }

        private async Task<OneOf<RequestGenerateTokenDTO, BaseError>> GetUserByCredentials(RequestUserLoginDTO content, CancellationToken cancellationToken = default)
        {
            var userToLogin = await unitOfWork.UserRepository.GetUserByCredentialsAsync(content, cancellationToken);
            if (userToLogin == null)
                return new UserOrPasswordIsIncorrectError();

            if (userToLogin.Status == EUserStatus.BANNED)
                return new UserIsBannedError();

            return userToLogin;
        }
    }
}
