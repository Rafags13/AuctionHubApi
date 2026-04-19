using AuctionHub.Domain.DTOs.User.Request.Create;
using AuctionHub.Domain.Errors.Common;
using OneOf;

namespace AuctionHub.Domain.Interfaces.Services.User.Register
{
    public interface IValidateRegisterService
    {
        Task<BaseError?> ValidateAsync(RequestCreateUserDTO content, CancellationToken cancellationToken);
    }
}
