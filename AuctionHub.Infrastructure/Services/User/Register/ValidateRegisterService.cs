using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Errors.Authentication.Register;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Helpers;
using AuctionHub.Domain.Helpers.Autentication;
using AuctionHub.Domain.Interfaces.Services.User.Register;
using AuctionHub.Domain.Interfaces.UoW;

namespace AuctionHub.Infrastructure.Services.User.Register
{
    internal sealed class ValidateRegisterService(IUnitOfWork unitOfWork) : IValidateRegisterService
    {
        public async Task<BaseError?> ValidateAsync(RequestRegisterUserDTO content, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(content.Name))
                return new NameIsRequiredError();

            if (string.IsNullOrWhiteSpace(content.Email))
                return new EmailIsRequiredError();

            if (!PasswordHelper.IsValid(content.Password))
                return new WeakPasswordError();

            if (!EmailHelper.IsValid(content.Email))
                return new InvalidEmailFormatError();

            if (await unitOfWork.UserRepository.ExistsByEmailAsync(content.Email, cancellationToken))
                return new UserAlreadyExistsError();

            return null;
        }
    }
}
