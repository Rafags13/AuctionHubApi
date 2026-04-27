using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.Channel.Auction.Open.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuctionHub.Infrastructure.Services.BackgroundServices.Auction.Open
{
    internal sealed class OpenAuctionBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        IBaseEventProducer<OpenAuctionEvent> auctionProducer
    ) : BackgroundService
    {
        private static readonly int SECONDS_TO_WAIT = 5;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var currentDate = DateTime.UtcNow;

                var scheduledAuctions = await repository.GetScheduledAuctionsToStartAsync(currentDate, stoppingToken);

                foreach (var scheduledAuction in scheduledAuctions)
                    await auctionProducer.DispatchAsync(new OpenAuctionEvent(scheduledAuction), stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(SECONDS_TO_WAIT), stoppingToken);
            }
        }
    }
}
