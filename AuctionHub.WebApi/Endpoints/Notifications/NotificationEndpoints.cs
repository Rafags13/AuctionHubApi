using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.DTOs.Notification.Paginated.Response;
using AuctionHub.Domain.DTOs.Notification.Read.Response;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.UseCases.Notification.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.WebApi.Endpoints.Notifications
{
    internal static class NotificationEndpoints
    {
        internal static IEndpointRouteBuilder AddNotificationEndpoints(this IEndpointRouteBuilder endpoint)
        {
            var root = endpoint.MapGroup("/api/notifications");

            root.MapGet("{id:long}", async (
                [FromServices] IGetNotificationDetailsUseCase useCase,
                [FromRoute] long id,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await useCase.GetAsync(id, cancellationToken);

                return result.Match(
                    success => Results.Ok(success),
                    error => Results.Json(error, statusCode: error.HttpErrorCode));
            })
                .WithDescription("Get the notification content.")
                .Produces<ReadNotificationResponseDTO>(StatusCodes.Status200OK)
                .Produces<BaseError>(StatusCodes.Status403Forbidden)
                .Produces<BaseError>(StatusCodes.Status404NotFound);

            root.MapGet("", async (
                [FromServices] IGetNotificationPaginatedUseCase useCase,
                [AsParameters] PaginatedNotificationRequestDTO content,
                CancellationToken cancellationToken = default
            ) =>
            {
                return Results.Ok(await useCase.GetPaginatedAsync(content, cancellationToken));
            })
                .WithDescription("Get notifications paginated.")
                .Produces<PaginatedDTO<PaginatedNotificationResponseDTO>>(StatusCodes.Status200OK);

            return endpoint;
        }
    }
}
