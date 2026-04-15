using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Infrastructure.Extensions
{
    public static class ConfigureMigrationExtensions
    {
        public static void ConfigureMigrations(this IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AuctionHubContext>();

            context?.Database.Migrate();
        }
    }
}
