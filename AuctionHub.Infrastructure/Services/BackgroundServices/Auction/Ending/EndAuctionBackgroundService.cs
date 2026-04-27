using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuctionHub.Infrastructure.Services.BackgroundServices.Auction.Ending
{
    internal sealed class EndAuctionBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        IBaseEventProducer<EndAuctionEvent> auctionProducer
    ) : BackgroundService
    {
        private static readonly int SECONDS_TO_WAIT = 5;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

            while(!stoppingToken.IsCancellationRequested)
            {
                var currentDate = DateTime.UtcNow;

                var expiredAuctions = await repository.GetExpiredAuctionsAsync(currentDate, stoppingToken);

                foreach (var expiredAuction in expiredAuctions)
                    await auctionProducer.DispatchAsync(new EndAuctionEvent(expiredAuction), stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(SECONDS_TO_WAIT), stoppingToken);
            }
        }
    }
}
