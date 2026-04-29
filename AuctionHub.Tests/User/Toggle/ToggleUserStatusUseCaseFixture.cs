using AuctionHub.Application.UseCases.User.Commands;
using AuctionHub.Domain.DTOs.User.Toggle.Request;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace AuctionHub.Tests.User.Toggle
{
    public class ToggleUserStatusUseCaseFixture
    {
        public IToggleStatusUserUseCase Create(ToggleUserStatusUseCaseMocks? mocks = null)
        {
            var unit = mocks?.UnitOfWork ?? new Moq.Mock<IUnitOfWork>();

            return new ToggleStatusUserUseCase(unit.Object);
        }

        public RequestToggleUserStatusDTO GetValidRequest()
            => new(1, EUserStatus.ACTIVE);
    }
}
