using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Consumer
{
    internal sealed class EndingAuctionEventConsumer(
        ILogger<EndingAuctionEventConsumer> logger,
        ChannelReader<EndAuctionEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AuctionHubContext>();
                var repository = new AuctionRepository(context);

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        if(!await repository.EndAsync(@event, stoppingToken))
                            logger.LogError("Ocorreu um erro ao finalizar o leilão {AuctionId}.", @event.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar evento no consumidor {ConsumerName}.", nameof(EndingAuctionEventConsumer));
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(EndingAuctionEventConsumer));
            }
        }
    }
}
