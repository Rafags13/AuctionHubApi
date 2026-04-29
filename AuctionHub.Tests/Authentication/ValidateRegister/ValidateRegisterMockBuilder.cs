using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Authentication.ValidateRegister
{
    public class ValidateRegisterMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private bool _userExists;

        public ValidateRegisterMockBuilder WithExistingUser()
        {
            _userExists = true;
            return this;
        }

        public ValidateRegisterMockBuilder WithNoExistingUser()
        {
            _userExists = false;
            return this;
        }

        public ValidateRegisterMocks Build()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.ExistsByEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_userExists);

            return new ValidateRegisterMocks(_unitOfWork);
        }
    }
}
