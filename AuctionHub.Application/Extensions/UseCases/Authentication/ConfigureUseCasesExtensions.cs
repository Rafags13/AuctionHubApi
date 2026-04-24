using AuctionHub.Application.UseCases.Authentication.Commands;
using AuctionHub.Domain.Interfaces.UseCases.Authentication.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases.Authentication
{
    internal static class ConfigureUseCasesExtensions
    {
        internal static IServiceCollection AddAuthenticationUseCases(this IServiceCollection services)
        {
            services.AddTransient<IRegisterSellerUseCase, RegisterSellerUseCase>();
            services.AddTransient<IRegisterBidderUseCase, RegisterBidderUseCase>();
            services.AddTransient<IUserLoginUseCase, UserLoginUseCase>();
            services.AddTransient<IRefreshTokenUseCase, RefreshTokenUseCase>();

            return services;
        }
    }
}
