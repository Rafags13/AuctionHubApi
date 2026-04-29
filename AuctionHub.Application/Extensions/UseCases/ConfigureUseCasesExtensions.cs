using AuctionHub.Application.Extensions.UseCases.Auction;
using AuctionHub.Application.Extensions.UseCases.Auction.Bid;
using AuctionHub.Application.Extensions.UseCases.Authentication;
using AuctionHub.Application.Extensions.UseCases.Notification;
using AuctionHub.Application.Extensions.UseCases.User;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases
{
    public static class ConfigureUseCasesExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            return services
                .AddAuthenticationUseCases()
                .AddUserUseCases()
                .AddAuctionUseCases()
                .AddNotificationUseCases();
        }
    }
}
