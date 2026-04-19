using AuctionHub.Application.UseCases.User.Commands;
using AuctionHub.Domain.Interfaces.UseCases.User.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases.User
{
    internal static class ConfigureUseCasesExtensions
    {
        internal static IServiceCollection AddUserUseCases(this IServiceCollection services)
        {
            services.AddTransient<IRegisterSellerUseCase, RegisterSellerUseCase>();
            services.AddTransient<IRegisterBidderUseCase, RegisterBidderUseCase>();
            services.AddTransient<IUserLoginUseCase, UserLoginUseCase>();

            return services;
        }
    }
}
