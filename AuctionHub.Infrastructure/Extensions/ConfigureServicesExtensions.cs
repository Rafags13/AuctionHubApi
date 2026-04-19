using AuctionHub.Domain.Interfaces.Services.User.Login;
using AuctionHub.Domain.Interfaces.Services.User.Password;
using AuctionHub.Domain.Interfaces.Services.User.Register;
using AuctionHub.Infrastructure.Services.User.Login;
using AuctionHub.Infrastructure.Services.User.Password;
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

            return services;
        }
    }
}
