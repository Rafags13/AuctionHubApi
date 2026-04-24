using AuctionHub.Domain.DTOs.User.Toggle.Request;
using AuctionHub.Domain.Enums.User;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using AuctionHub.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.WebApi.Endpoints.User
{
    internal static class UserEndpoints
    {
        internal static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var root = endpoints.MapGroup("/api/users")
                .WithTags("Users");

            root.MapPatch("toggle", async (
                [FromServices] IToggleStatusUserUseCase useCase,
                [FromBody] RequestToggleUserStatusDTO body,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.ToggleAsync(body, cancellationToken);

                return result.Match(
                    _ => Results.NoContent(),
                    error => Results.Json(error, statusCode: error.HttpErrorCode)
                );
            })
                .WithDescription("Toggle user's status.")
                .Produces(StatusCodes.Status204NoContent)
                .Produces<BaseError>(StatusCodes.Status404NotFound)
                .Produces<BaseError>(StatusCodes.Status500InternalServerError)
                .Authorize(ERole.ADMIN);

            return endpoints;
        }
    }
}
