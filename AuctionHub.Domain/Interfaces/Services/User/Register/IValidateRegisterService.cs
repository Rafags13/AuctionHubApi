using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.Errors.Common.Base;
using OneOf;

namespace AuctionHub.Domain.Interfaces.Services.User.Register
{
    public interface IValidateRegisterService
    {
        Task<BaseError?> ValidateAsync(RequestRegisterUserDTO content, CancellationToken cancellationToken);
    }
}
