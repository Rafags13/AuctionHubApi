using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Common.User
{
    public record UserNotFoundError() : BaseError("User not found.", nameof(UserNotFoundError), StatusCodes.Status404NotFound);
}
