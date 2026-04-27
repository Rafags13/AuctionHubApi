using AuctionHub.Application.UseCases.Notification.Queries;
using AuctionHub.Domain.Interfaces.UseCases.Notification.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases.Notification
{
    internal static class ConfigureUseCasesExtensions
    {
        internal static IServiceCollection AddNotificationUseCases(this IServiceCollection services)
        {
            services.AddTransient<IGetNotificationDetailsUseCase, GetNotificationDetailsUseCase>();
            services.AddTransient<IGetNotificationPaginatedUseCase, GetNotificationPaginatedUseCase>();

            return services;
        }
    }
}
