using AuctionHub.Application.UseCases.User.Commands;
using AuctionHub.Application.UseCases.User.Queries;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using AuctionHub.Domain.Interfaces.UseCases.User.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases.User
{
    internal static class ConfigureUseCasesExtensions
    {
        internal static IServiceCollection AddUserUseCases(this IServiceCollection services)
        {
            services.AddTransient<IToggleStatusUserUseCase, ToggleStatusUserUseCase>();
            services.AddTransient<IGetUserProfileUseCase, GetUserProfileUseCase>();

            return services;
        }
    }
}
