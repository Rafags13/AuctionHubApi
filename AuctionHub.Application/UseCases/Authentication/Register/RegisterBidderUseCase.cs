using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Errors.Common;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using AuctionHub.Domain.Interfaces.Services.User.Register;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Register;
using OneOf;

namespace AuctionHub.Application.UseCases.Authentication.Register
{
    internal sealed class RegisterBidderUseCase(
        IUnitOfWork unitOfWork,
        IValidateRegisterService validateRegisterService,
        IPasswordHashService passwordHashService
    ) : IRegisterBidderUseCase
    {
        public async Task<OneOf<bool, BaseError>> RegisterAsync(RequestCreateBidderDTO content, CancellationToken cancellationToken)
        {
            var error = await validateRegisterService.ValidateAsync(content, cancellationToken);
            if (error != null)
                return error;

            var hashedPassword = passwordHashService.GenerateHash(content.Password);

            if (!await unitOfWork.UserRepository.CreateAsync(content, hashedPassword, cancellationToken))
                return new DatabaseError();

            return true;
        }
    }
}
