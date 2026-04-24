using AuctionHub.Domain.Constants.Authentication.Login;
using AuctionHub.Domain.Constants.Authentication.Password;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuctionHub.Infrastructure.Extensions
{
    public static class ConfigureInfrastructureExtensions
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.ConfigureRepository(configuration)
                .ConfigureContants(configuration);
        }

        private static IServiceCollection ConfigureRepository(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONTEXT_DATA_SOURCE")
                                       ?? configuration.GetConnectionString("CONTEXT_DATA_SOURCE")
                                       ?? throw new ArgumentNullException("Não foi possível encontrar a string de conexão para CONTEXT_DATA_SOURCE");

            services.AddDbContext<AuctionHubContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection ConfigureContants(this IServiceCollection services, IConfiguration configuration)
        {
            PasswordConstants.HASH = Environment.GetEnvironmentVariable("PASSWORD_HASH") ??
                configuration.GetConnectionString("CONTEXT_DATA_SOURCE") ??
                throw new ArgumentNullException("Não foi possível encontrar a variável de ambiente PASSWORD_HASH");

            AuthenticationJwtConstants.SECRET_KEY = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ??
                configuration.GetConnectionString("JWT_SECRET_KEY") ??
                throw new ArgumentNullException("Não foi possível encontrar a variável de ambiente JWT_SECRET_KEY");

            return services;
        }
    }
}
