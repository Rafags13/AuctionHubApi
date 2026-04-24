using AuctionHub.Domain.Enums.User;
using Microsoft.AspNetCore.Authorization;

namespace AuctionHub.WebApi.Extensions
{
    internal static class AuthorizeEndpointsExtensions
    {
        internal static IEndpointConventionBuilder Authorize(this IEndpointConventionBuilder builder, params ERole[] roles)
        {
            var authorizeAttribute = new AuthorizeAttribute { Roles = string.Join(",", roles) };
            return builder.RequireAuthorization(authorizeAttribute);
        }
    }
}
