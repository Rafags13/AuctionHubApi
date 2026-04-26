using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Common.Authentication
{
    public record UserIsNotAuthorizedError() : BaseError("User is not authorized to perform this action.", nameof(UserIsNotAuthorizedError), StatusCodes.Status403Forbidden);
}
