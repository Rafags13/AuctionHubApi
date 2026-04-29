using AuctionHub.Application.UseCases.Authentication.Commands;
using AuctionHub.Application.UseCases.Authentication.Register;
using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Register;

namespace AuctionHub.Tests.Authentication.Register.Bidder
{
    public class RegisterBidderFixture
    {
        public IRegisterBidderUseCase Create(RegisterBidderMocks mocks)
        {
            return new RegisterBidderUseCase(
                mocks.UnitOfWork.Object,
                mocks.ValidateRegisterService.Object,
                mocks.PasswordHashService.Object
            );
        }

        public RequestCreateBidderDTO CreateValidRequest()
            => new("Test User", "test@email.com", "Strong@123");
    }
}
