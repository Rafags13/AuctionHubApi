using AuctionHub.Domain.DTOs.Authentication.Login.Response;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Authentication.Login;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.Services.Authentication.Login;
using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands;
using AuctionHub.Infrastructure.Extensions;
using OneOf;

namespace AuctionHub.Application.UseCases.Authentication.Commands
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
