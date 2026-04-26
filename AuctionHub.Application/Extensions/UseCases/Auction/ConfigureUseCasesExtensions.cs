using AuctionHub.Application.Extensions.UseCases.Auction.Bid;
using AuctionHub.Application.UseCases.Auction.Create.Commands;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Create.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases.Auction
{
    internal static class ConfigureUseCasesExtensions
    {
        internal static IServiceCollection AddAuctionUseCases(this IServiceCollection services)
        {
            services.AddTransient<ICreateAuctionUseCase, CreateAuctionUseCase>();

            return services.AddBidUseCases();
        }
    }
}
