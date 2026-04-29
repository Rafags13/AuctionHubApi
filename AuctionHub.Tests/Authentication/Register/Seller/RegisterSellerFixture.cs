using AuctionHub.Application.UseCases.Authentication.Commands;
using AuctionHub.Application.UseCases.Authentication.Register;
using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Register;

namespace AuctionHub.Tests.Authentication.Register.Seller
{
    public class RegisterSellerFixture
    {
        public IRegisterSellerUseCase Create(RegisterSellerMocks mocks)
        {
            return new RegisterSellerUseCase(
                mocks.UnitOfWork.Object,
                mocks.ValidateRegisterService.Object,
                mocks.PasswordHashService.Object
            );
        }

        public RequestCreateSellerDTO CreateValidRequest()
            => new("Test Seller", "seller@email.com", "Strong@123");
    }
}
