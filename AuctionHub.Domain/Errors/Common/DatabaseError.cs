using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Common
{
    public record DatabaseError() : BaseError("An internal error occurred. Try again later", nameof(DatabaseError), StatusCodes.Status500InternalServerError);
}
