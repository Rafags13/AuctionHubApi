using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Common.Notification
{
    public record NotificationNotFoundError() : BaseError("Notification not found.", nameof(NotificationNotFoundError), StatusCodes.Status404NotFound);
}
