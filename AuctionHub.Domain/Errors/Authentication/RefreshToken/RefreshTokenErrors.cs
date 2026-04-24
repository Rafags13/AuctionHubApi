using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Authentication.RefreshToken
{
    public record InvalidRefreshTokenError() : BaseError("Invalid token.", nameof(InvalidRefreshTokenError), StatusCodes.Status401Unauthorized);
    public record ExpiredRefreshTokenError() : BaseError("Expired token.", nameof(ExpiredRefreshTokenError), StatusCodes.Status401Unauthorized);
}
