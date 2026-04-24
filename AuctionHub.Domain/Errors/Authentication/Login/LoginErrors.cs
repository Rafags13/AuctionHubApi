using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Authentication.Login
{
    public record UserOrPasswordIsIncorrectError() : BaseError("The email or password provided is incorrect.", nameof(UserOrPasswordIsIncorrectError), StatusCodes.Status401Unauthorized);
    public record UserIsBannedError() : BaseError("Current user is permanently banned.", nameof(UserIsBannedError), StatusCodes.Status403Forbidden);
}
