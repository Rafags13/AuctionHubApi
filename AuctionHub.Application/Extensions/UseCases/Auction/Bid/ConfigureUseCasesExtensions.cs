using AuctionHub.Application.UseCases.Auction.Bid.Create.Commands;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Bid.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases.Auction.Bid
{
    internal static class ConfigureUseCasesExtensions
    {
        internal static IServiceCollection AddBidUseCases(this IServiceCollection services)
        {
            services.AddTransient<ICreateBidUseCase, CreateBidUseCase>();

            return services;
        }
    }
}
