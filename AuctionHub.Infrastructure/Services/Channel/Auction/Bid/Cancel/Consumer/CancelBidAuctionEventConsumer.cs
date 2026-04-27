using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Consumer
{
    internal sealed class CancelBidAuctionEventConsumer(
        ILogger<CancelBidAuctionEventConsumer> logger,
        ChannelReader<CancelBidAuctionEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var bidRepository = scope.ServiceProvider.GetRequiredService<IBidRepository>();

                await foreach(var @event in channel.ReadAllAsync(stoppingToken))
                {
                    if(!await bidRepository.CancelAsync(@event.Id, stoppingToken))
                    {
                        logger.LogWarning("Falha ao cancelar o lance com ID {BidId}.", @event.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(BidAuctionEventConsumer));
            }
        }
    }
}
