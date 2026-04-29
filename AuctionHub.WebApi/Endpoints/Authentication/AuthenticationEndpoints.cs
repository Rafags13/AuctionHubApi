using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Register;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.WebApi.Endpoints.Authentication
{
    internal static class AuthenticationEndpoints
    {
        internal static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var root = endpoints.MapGroup("/api/auth")
                .WithTags("Authentication");

            root.MapPost("/login", async (
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
                .Produces<BaseError>(StatusCodes.Status401Unauthorized)
                .AllowAnonymous();

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
                .Produces<BaseError>(StatusCodes.Status500InternalServerError)
                .AllowAnonymous();

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
                .Produces<BaseError>(StatusCodes.Status500InternalServerError)
                .AllowAnonymous();

            root.MapPatch("/refresh-token", async (
                [FromServices] IRefreshTokenUseCase useCase,
                [FromQuery] string refreshToken,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.RefreshTokenAsync(refreshToken, cancellationToken);

                return result.Match(
                    success => Results.Ok(success),
                    error => Results.Json(error, statusCode: error.HttpErrorCode)
                );
            })
                .WithDescription("Renew refresh token.")
                .Produces<string>(StatusCodes.Status200OK)
                .Produces<BaseError>(StatusCodes.Status401Unauthorized)
                .Produces<BaseError>(StatusCodes.Status500InternalServerError)
                .AllowAnonymous();

            return endpoints;
        }
    }
}
