using AuctionHub.WebApi.Endpoints.User;

namespace AuctionHub.WebApi.Extensions
{
    internal static class ConfigureEndpointExtensions
    {
        internal static IEndpointRouteBuilder AddEndpoints(this IEndpointRouteBuilder endpoint)
        {
            return endpoint.AddUserEndpoints();
        }
    }
}
