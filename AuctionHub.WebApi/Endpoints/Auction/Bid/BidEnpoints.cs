using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Bid.Commands;
using AuctionHub.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.WebApi.Endpoints.Auction.Bid
{
    internal static class BidEnpoints
    {
        internal static IEndpointRouteBuilder AddBidEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var root = endpoints.MapGroup("/api/auctions/bids")
                .WithTags("Bids");

            root.MapPost("", async (
                [FromServices] ICreateBidUseCase useCase,
                [FromBody] BidRequestDTO body,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.BidAsync(body, cancellationToken);

                return result.Match(
                    _ => Results.Created(),
                    error => Results.Json(error, statusCode: error.HttpErrorCode));
            })
                .WithDescription("Creates a bids on the auction.")
                .Produces(StatusCodes.Status201Created)
                .Produces<BaseError>(StatusCodes.Status400BadRequest)
                .Produces<BaseError>(StatusCodes.Status403Forbidden)
                .Produces<BaseError>(StatusCodes.Status404NotFound)
                .Authorize(ERole.BIDDER);

            return endpoints;
        }
    }
}
