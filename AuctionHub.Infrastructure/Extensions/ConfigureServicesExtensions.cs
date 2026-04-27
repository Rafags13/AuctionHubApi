using AuctionHub.Domain.Interfaces.Services.Authentication.Login;
using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using AuctionHub.Domain.Interfaces.Services.Caching;
using AuctionHub.Domain.Interfaces.Services.External.Payment;
using AuctionHub.Domain.Interfaces.Services.User.Register;
using AuctionHub.Infrastructure.Services.Authentication.Login;
using AuctionHub.Infrastructure.Services.Authentication.Password;
using AuctionHub.Infrastructure.Services.Caching;
using AuctionHub.Infrastructure.Services.External.Payment;
using AuctionHub.Infrastructure.Services.User.Register;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Infrastructure.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<IValidateRegisterService, ValidateRegisterService>();

            services.AddScoped<IGenerateTokenService, GenerateTokenService>();

            services.AddScoped<ICachingService, CachingService>();

            services.AddScoped<IExternalIntegrationPaymentService, ExternalIntegrationPaymentService>();

            return services;
        }
    }
}
