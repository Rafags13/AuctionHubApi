using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Errors.Common;
using AuctionHub.Domain.Errors.User;
using AuctionHub.Domain.Interfaces.Services.User.Login;
using AuctionHub.Domain.Interfaces.Services.User.Password;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using OneOf;

namespace AuctionHub.Application.UseCases.User.Commands
{
    internal sealed class UserLoginUseCase(
        IUnitOfWork unitOfWork,
        IPasswordHashService passwordHashService,
        IGenerateTokenService generateTokenService) : IUserLoginUseCase
    {
        public async Task<OneOf<string, BaseError>> LoginAsync(RequestUserLoginDTO content, CancellationToken cancellationToken = default)
        {
            var hashPassword = passwordHashService.GenerateHash(content.Password);
            content.HashPassword(hashPassword);

            var userToLogin = await unitOfWork.UserRepository.GetUserByCredentialsAsync(content, cancellationToken);
            if (userToLogin == null)
                return new UserOrPasswordIsIncorrectError();

            return generateTokenService.GenerateToken(userToLogin);
        }
    }
}
