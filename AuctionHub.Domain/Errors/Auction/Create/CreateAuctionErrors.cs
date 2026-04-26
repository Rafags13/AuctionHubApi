using AuctionHub.Domain.Errors.Common.Base;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Domain.Errors.Auction.Create
{
    public record TitleIsRequiredError() : BaseError("Title is required.", nameof(TitleIsRequiredError), StatusCodes.Status400BadRequest);

    public record MaxTitleSizeError(int Size) : BaseError($"Title must be less than {Size} characters.", nameof(MaxTitleSizeError), StatusCodes.Status400BadRequest);

    public record DescriptionIsRequiredError() : BaseError("Description is required.", nameof(DescriptionIsRequiredError), StatusCodes.Status400BadRequest);

    public record MaxDescriptionSizeError(int Size) : BaseError($"Description must be less than {Size} characters.", nameof(MaxDescriptionSizeError), StatusCodes.Status400BadRequest);

    public record StartingPriceMustBeGreaterThanZeroError() : BaseError("Starting price must be greater than zero.", nameof(StartingPriceMustBeGreaterThanZeroError), StatusCodes.Status400BadRequest);

    public record StartTimeMustBeInTheFutureError() : BaseError("Start time must be in the future.", nameof(StartTimeMustBeInTheFutureError), StatusCodes.Status400BadRequest);
    public record UserIsntASellerError() : BaseError("User isn't a seller.", nameof(UserIsntASellerError), StatusCodes.Status400BadRequest);
}
