using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Create.Commands;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.WebApi.Endpoints.Auction
{
    internal static class AuctionEndpoints
    {
        internal static IEndpointRouteBuilder AddAuctionEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var root = endpoints.MapGroup("/api/auctions")
                .WithTags("Auctions");

            root.MapPost("", async (
                [FromServices] ICreateAuctionUseCase useCase,
                [FromBody] RequestCreateAuctionDTO content,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.CreateAsync(content, cancellationToken);

                return result.Match(
                    _ => Results.Created(),
                    error => Results.Json(error, statusCode: error.HttpErrorCode));
            })
                .WithDescription("Creates an auction.")
                .Produces(StatusCodes.Status201Created)
                .Produces<BaseError>(StatusCodes.Status400BadRequest)
                .Produces<BaseError>(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}
