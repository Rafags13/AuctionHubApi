using AuctionHub.Application.Extensions.UseCases.Auction.Bid;
using AuctionHub.Application.UseCases.Auction.Create.Commands;
using AuctionHub.Application.UseCases.Auction.Details.Queries;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Create.Commands;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Details.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionHub.Application.Extensions.UseCases.Auction
{
    internal static class ConfigureUseCasesExtensions
    {
        internal static IServiceCollection AddAuctionUseCases(this IServiceCollection services)
        {
            services.AddTransient<ICreateAuctionUseCase, CreateAuctionUseCase>();
            services.AddTransient<IGetAuctionInformationsUseCase, GetAuctionInformationsUseCase>();

            return services.AddBidUseCases();
        }
    }
}
