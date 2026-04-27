using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Create.Consumer
{
    internal sealed class CreateAuctionEventConsumer(
        ILogger<CreateAuctionEventConsumer> logger,
        ChannelReader<CreateAuctionEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        await repository.CreateAsync(@event, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar evento no consumidor {ConsumerName}.", nameof(CreateAuctionEventConsumer));
                    }
                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(CreateAuctionEventConsumer));
            }
        }
    }
}
