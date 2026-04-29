using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Interfaces.Services.User.Register;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Services.User.Register;
using Moq;

namespace AuctionHub.Tests.Authentication.ValidateRegister
{
    public class ValidateRegisterFixture
    {
        public IValidateRegisterService Create(ValidateRegisterMocks? mocks = null)
        {
            var unit = mocks?.UnitOfWork ?? new Mock<IUnitOfWork>();

            return new ValidateRegisterService(unit.Object);
        }

        public RequestCreateBidderDTO CreateValidRequest()
            => new("Test User", "test@email.com", "Strong@123");
    }
}
