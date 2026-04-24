using AuctionHub.Domain.Errors.Common;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.User
{
    public record UserAlreadyExistsError() : BaseError("User with the same email already exists.", nameof(BaseError), StatusCodes.Status400BadRequest);
    public record InvalidEmailFormatError() : BaseError("The provided email format is invalid.", nameof(InvalidEmailFormatError), StatusCodes.Status400BadRequest);
    public record WeakPasswordError() : BaseError("The provided password does not meet the strength requirements.", nameof(WeakPasswordError), StatusCodes.Status400BadRequest);
    public record NameIsRequiredError(): BaseError("The name field is required.", nameof(NameIsRequiredError), StatusCodes.Status400BadRequest);
    public record EmailIsRequiredError(): BaseError("The email field is required.", nameof(EmailIsRequiredError), StatusCodes.Status400BadRequest);
    public record PasswordIsRequiredError(): BaseError("The password field is required.", nameof(PasswordIsRequiredError), StatusCodes.Status400BadRequest);
    public record UserOrPasswordIsIncorrectError() : BaseError("The email or password provided is incorrect.", nameof(UserOrPasswordIsIncorrectError), StatusCodes.Status401Unauthorized);
    public record UserIsBannedError() : BaseError("Current user is permanently banned.", nameof(UserIsBannedError), StatusCodes.Status403Forbidden);
    public record UserNotFoundError() : BaseError("User not found.", nameof(UserNotFoundError), StatusCodes.Status404NotFound);
    public record InvalidRefreshTokenError() : BaseError("Invalid token.", nameof(InvalidRefreshTokenError), StatusCodes.Status401Unauthorized);
    public record ExpiredRefreshTokenError() : BaseError("Expired token.", nameof(ExpiredRefreshTokenError), StatusCodes.Status401Unauthorized);
}
