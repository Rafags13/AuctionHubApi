using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Notification.Read
{
    public record NotificationDoesntBelongToCurrentUserError() :
        BaseError("You can only read your notifications!", nameof(NotificationDoesntBelongToCurrentUserError), StatusCodes.Status403Forbidden)
}
