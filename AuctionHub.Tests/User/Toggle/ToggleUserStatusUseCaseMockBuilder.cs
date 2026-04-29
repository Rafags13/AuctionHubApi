using AuctionHub.Domain.DTOs.User.Toggle.Request;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Tests.Authentication.ValidateRegister;
using FluentAssertions;
using Moq;

namespace AuctionHub.Tests.User.Toggle
{
    public class ToggleUserStatusUseCaseMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private bool _userExists;
        private int _successfulToggle;

        public ToggleUserStatusUseCaseMockBuilder UserExists(bool successOperation)
        {
            _userExists = successOperation;

            return this;
        }

        public ToggleUserStatusUseCaseMockBuilder SuccessfulToggle(bool successOperation)
        {
            _successfulToggle = successOperation ? 1 : 0;

            return this;
        }

        public ToggleUserStatusUseCaseMocks Build()
        {
            _unitOfWork
                .Setup(x => x.UserRepository.FindByIdAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_userExists);

            _unitOfWork
                .Setup(x => x.UserRepository.ToggleAsync(
                    It.IsAny<RequestToggleUserStatusDTO>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_successfulToggle);

            return new ToggleUserStatusUseCaseMocks(_unitOfWork);
        }
    }
}
