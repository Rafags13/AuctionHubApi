using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using AuctionHub.Domain.Interfaces.Services.User.Register;
using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Authentication.Register.Seller
{
    public record RegisterSellerMocks(
        Mock<IUnitOfWork> UnitOfWork,
        Mock<IValidateRegisterService> ValidateRegisterService,
        Mock<IPasswordHashService> PasswordHashService
    );
}
