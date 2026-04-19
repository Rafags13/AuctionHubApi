using AuctionHub.Application.Extensions.UseCases.User;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases
{
    public static class ConfigureUseCasesExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            return services.AddUserUseCases();
        }
    }
}
