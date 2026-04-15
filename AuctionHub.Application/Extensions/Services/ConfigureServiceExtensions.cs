using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.Services
{
    public static class ConfigureServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
