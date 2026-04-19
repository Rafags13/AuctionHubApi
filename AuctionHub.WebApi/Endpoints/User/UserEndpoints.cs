using AuctionHub.Domain.DTOs.User.Request.Create;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Errors.Common;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.WebApi.Endpoints.User
{
    internal static class UserEndpoints
    {
        internal static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var root = endpoints.MapGroup("/api/users")
                .WithTags("Users");

            root.MapPost("/bidder", async (
                [FromServices] IRegisterBidderUseCase useCase,
                [FromBody] RequestCreateBidderDTO body,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.RegisterAsync(body, cancellationToken);

                return result.Match(
                    _ => Results.Created(),
                    error => Results.Json(error, statusCode: error.HttpErrorCode)
                );
            })
                .WithDescription("Creates a new bidder.")
                .Produces<bool>(StatusCodes.Status200OK)
                .Produces<BaseError>(StatusCodes.Status400BadRequest)
                .Produces<BaseError>(StatusCodes.Status500InternalServerError);

            root.MapPost("/seller", async (
                [FromServices] IRegisterSellerUseCase useCase,
                [FromBody] RequestCreateSellerDTO body,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.RegisterAsync(body, cancellationToken);

                return result.Match(
                    _ => Results.Created(),
                    error => Results.Json(error, statusCode: error.HttpErrorCode)
                );
            })
                .WithDescription("Creates a new seller.")
                .Produces<bool>(StatusCodes.Status200OK)
                .Produces<BaseError>(StatusCodes.Status400BadRequest)
                .Produces<BaseError>(StatusCodes.Status500InternalServerError);

            root.MapPost("login", async (
                [FromServices] IUserLoginUseCase useCase,
                [FromBody] RequestUserLoginDTO body,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.LoginAsync(body, cancellationToken);

                return result.Match(
                    success => Results.Ok(success),
                    error => Results.Json(error, statusCode: error.HttpErrorCode)
                );
            })
                .WithDescription("Log-in an user.")
                .Produces<string>(StatusCodes.Status200OK)
                .Produces<BaseError>(StatusCodes.Status401Unauthorized);

            return endpoints;
        }
    }
}
