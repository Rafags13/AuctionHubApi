using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using AuctionHub.Domain.Interfaces.Services.User.Register;
using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Authentication.Register.Bidder
{
    public class RegisterBidderMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IValidateRegisterService> _validateService = new();
        private readonly Mock<IPasswordHashService> _passwordHashService = new();

        private BaseError? _validationError;
        private bool _creationResult = true;

        public RegisterBidderMockBuilder()
        {
            _passwordHashService
                .Setup(x => x.GenerateHash(It.IsAny<string>()))
                .Returns("hashed");
        }

        public RegisterBidderMockBuilder WithValidValidation()
        {
            _validationError = null;
            return this;
        }

        public RegisterBidderMockBuilder WithValidationError(BaseError error)
        {
            _validationError = error;
            return this;
        }

        public RegisterBidderMockBuilder WithSuccessfulCreation()
        {
            _creationResult = true;
            return this;
        }

        public RegisterBidderMockBuilder WithCreationFailure()
        {
            _creationResult = false;
            return this;
        }

        public RegisterBidderMocks Build()
        {
            _validateService
                .Setup(x => x.ValidateAsync(It.IsAny<RequestRegisterUserDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_validationError);

            _unitOfWork
                .Setup(x => x.UserRepository.CreateAsync(
                    It.IsAny<RequestCreateBidderDTO>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_creationResult);

            return new RegisterBidderMocks(
                _unitOfWork,
                _validateService,
                _passwordHashService
            );
        }
    }
}
